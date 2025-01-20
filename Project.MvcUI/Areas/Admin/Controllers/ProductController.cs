using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Project.Bll.DtoClasses;
using Project.Bll.Managers.Abstracts;
using Project.Entities.Models;
using Project.MvcUI.Areas.Admin.Models.PageVms;

namespace Project.MvcUI.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles ="Admin")]
    public class ProductController : Controller
    {

        readonly IProductManager _productManager;
        readonly ICategoryManager _categoryManager;
        IMapper _mapper;

        public ProductController(IProductManager productManager, ICategoryManager categoryManager, IMapper mapper)
        {
            _productManager = productManager;
            _categoryManager = categoryManager;
            _mapper = mapper;
        }

        public async Task<IActionResult> Index()
        {
            List<ProductDto> products = await _productManager.GetAllAsync();
            //Sakın kullanıcıya Domain Entity yollamayın...
            List<Product> productListModel = _mapper.Map<List<Product>>(products);
            return View(productListModel);
        }

        public IActionResult Create()
        {
            CreateProductPageVm cpVm = new()
            {
                Categories = _mapper.Map<List<Category>>(_categoryManager.GetActives())
            };
            return View(cpVm);
        }

        [HttpPost]
        public async Task<IActionResult> Create(Product product,IFormFile formFile)
        {
            #region ResimKodlari

            Guid uniqueName = Guid.NewGuid();
            string extension = Path.GetExtension(formFile.FileName); //dosyanın uzantısını ele gecirdik
            product.ImagePath = $"/images/{uniqueName}{extension}";

            string path = $"{Directory.GetCurrentDirectory()}/wwwroot/{product.ImagePath}";
            FileStream stream = new(path, FileMode.Create);
            formFile.CopyTo(stream);

            #endregion
            await _productManager.CreateAsync(_mapper.Map<ProductDto>(product));
            return RedirectToAction("Index");
        }
    }
}
