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




        public ShoppingController(IProductManager productManager, ICategoryManager categoryManager, IOrderManager orderManager, IOrderDetailManager orderDetailManager, UserManager<AppUser> userManager, IMapper mapper)
        {
            _productManager = productManager;
            _categoryManager = categoryManager;
            _orderManager = orderManager;
            _orderDetailManager = orderDetailManager;
            _userManager = userManager;
            _mapper = mapper;

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
                CategoryName = productToBeAdded.Category.CategoryName
            };

            c.AddToCart(ci);

            SetCartForSession(c);
            TempData["Message"] = $"{ci.ProductName} isimli ürün sepete eklenmiştir";
            return RedirectToAction("Index");
        }
    }
}
