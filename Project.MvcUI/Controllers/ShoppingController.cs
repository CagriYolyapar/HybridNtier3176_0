using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Project.Bll.DtoClasses;
using Project.Bll.Managers.Abstracts;
using Project.Dal.Repositories.Abstracts;
using Project.Entities.Models;
using Project.MvcUI.Models.PageVms;
using X.PagedList;
using X.PagedList.Extensions;
using Project.MvcUI.Models.SessionService;
using Project.MvcUI.Models.ShoppingTools;
using Newtonsoft.Json;
using System.Text;

namespace Project.MvcUI.Controllers
{
    public class ShoppingController : Controller
    {
        readonly IProductManager _productManager;
        readonly ICategoryManager _categoryManager;
        readonly IOrderManager _orderManager;
        readonly IOrderDetailManager _orderDetailManager;
        readonly UserManager<AppUser> _userManager;
        readonly IMapper _mapper;
        readonly IHttpClientFactory _httpClientFactory;




        public ShoppingController(IProductManager productManager, ICategoryManager categoryManager, IOrderManager orderManager, IOrderDetailManager orderDetailManager, UserManager<AppUser> userManager, IMapper mapper, IHttpClientFactory httpClientFactory)
        {
            _productManager = productManager;
            _categoryManager = categoryManager;
            _orderManager = orderManager;
            _orderDetailManager = orderDetailManager;
            _userManager = userManager;
            _mapper = mapper;
            _httpClientFactory = httpClientFactory;
        }

        //Todo Api icin IHttpClientFactory

        public async Task<IActionResult> Index(int? page, int? categoryId)
        {

            //string a = "Cagri";

            //string b = a ?? "Deneme"; eger a null ise "Deneme" string verisini , null degilse a'nın kendi degerini b'ye ata  demektir...

            //List<ProductDto> products = categoryId == null ? _productManager.GetActives() : _productManager.Where(x => x.CategoryId == categoryId);

            //List<Product> pagedProductsList = _mapper.Map<List<Product>>(products);
            List<Product> pagedProductsList = categoryId == null ? await _productManager.GetDomainProducts() : _productManager.GetDomainProductsByCategoryId(categoryId.Value);


            IPagedList<Product> pagedProducts = pagedProductsList.ToPagedList(page ?? 1, 5);

            List<Category> categories = _mapper.Map<List<Category>>(_categoryManager.GetActives());

            ShoppingPageVm spVm = new()
            {
                Products = pagedProducts,
                Categories = categories
            };

            if (categoryId != null) TempData["catId"] = categoryId;


            return View(spVm);
        }

        //Dikkat edin bu metotlar private'tir...Yani hicbir zaman bir Request'e cevap vermeyeceklerdir...Onlar sadece ve sadece Controller'in icerisinden ulasılabilen Controller'a özel korunmus yapılardır...

        #region ControllerPrivateMetotlari
        void SetCartForSession(Cart c)
        {
            HttpContext.Session.SetObject("scart", c);
        }

        Cart GetCartFromSession(string key)
        {
            return HttpContext.Session.GetObject<Cart>(key);
        }

        void ControlCart(Cart c)
        {
            if (c.GetCartItems.Count == 0) HttpContext.Session.Remove("scart");
        }

        #endregion
        public async Task<IActionResult> AddToCart(int id)
        {
            Cart c = GetCartFromSession("scart") == null ? new Cart() : GetCartFromSession("scart");

            Product productToBeAdded = _mapper.Map<Product>(await _productManager.GetByIdAsync(id));

            CartItem ci = new()
            {
                Id = productToBeAdded.Id,
                ProductName = productToBeAdded.ProductName,
                UnitPrice = productToBeAdded.UnitPrice,
                ImagePath = productToBeAdded.ImagePath,
                CategoryId = productToBeAdded.CategoryId,
                CategoryName = productToBeAdded.Category == null ? "Kategorisi yok" : productToBeAdded.Category.CategoryName
            };

            c.AddToCart(ci);

            SetCartForSession(c);
            TempData["Message"] = $"{ci.ProductName} isimli ürün sepete eklenmiştir";
            return RedirectToAction("Index");
        }

        public IActionResult CartPage()
        {
            if (GetCartFromSession("scart") == null)
            {
                TempData["Message"] = "Sepetiniz su anda bos";
                return RedirectToAction("Index");
            }
            Cart c = GetCartFromSession("scart");
            return View(c);
        }

        //Asagıdaki Session ve Sepet işlemlerini refactor ediniz...
        public IActionResult RemoveFromCart(int id)
        {
            if (GetCartFromSession("scart") != null)
            {
                Cart c = GetCartFromSession("scart");
                c.RemoveFromCart(id);
                SetCartForSession(c);
                ControlCart(c);
            }

            return RedirectToAction("CartPage");
        }

        public IActionResult DecreaseFromCart(int id)
        {
            if (GetCartFromSession("scart") != null)
            {
                Cart c = GetCartFromSession("scart");
                c.Decrease(id);
                SetCartForSession(c);
                ControlCart(c);
            }
            return RedirectToAction("CartPage");
        }

        public IActionResult IncreaseCartItem(int id)
        {
            if (GetCartFromSession("scart") != null)
            {
                Cart c = GetCartFromSession("scart");
                c.IncreaseCartItem(id);
                SetCartForSession(c);
                ControlCart(c);
            }

            return RedirectToAction("CartPage");
        }

        public IActionResult ConfirmOrder()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> ConfirmOrder(OrderRequestPageVm ovm)
        {
            Cart c = GetCartFromSession("scart");

           
            ovm.Order.Price = ovm.PaymentRequestModel.ShoppingPrice = c.TotalPrice;





            #region APIIntegration

            //http://localhost:5107/

            //API Entegrasyonu

            //API'yı consume etmek icin middleware'de ilk basta bir ayarlama yapmak zorundasınız...

            HttpClient client = _httpClientFactory.CreateClient();
            string jsonData = JsonConvert.SerializeObject(ovm.PaymentRequestModel);

            //Content : icerik

            StringContent content = new StringContent(jsonData,Encoding.UTF8,"application/json");

           HttpResponseMessage responseMessage =  await client.PostAsync("http://localhost:5107/api/Transaction", content);

            if (responseMessage.IsSuccessStatusCode)
            {
                if (User.Identity.IsAuthenticated)
                {
                    AppUser appUser = await _userManager.FindByNameAsync(User.Identity.Name);
                    ovm.Order.AppUserId = appUser.Id;
                }

                OrderDto orderDto = _mapper.Map<OrderDto>(ovm.Order);
                int orderId = await _orderManager.CreateOrderAndReturn(orderDto);

                foreach (CartItem item in c.GetCartItems)
                {
                    OrderDetail od = new();
                    od.OrderId = orderId;
                    od.ProductId = item.Id;
                    od.Quantity = item.Amount;
                    od.UnitPrice = item.UnitPrice;

                    await _orderDetailManager.CreateAsync(_mapper.Map<OrderDetailDto>(od));
                    //ürün stoktan düsmeli
                }

                TempData["Message"] = "Siparişiniz bize basarıyla ulasmıstır...Tesekkür ederiz";
                HttpContext.Session.Remove("scart"); //Session'i silme kodu

                return RedirectToAction("Index");
            }

            string result = await responseMessage.Content.ReadAsStringAsync();
            TempData["Message"] = result;
            return RedirectToAction("Index");

            #endregion





           
        }
    }
}
