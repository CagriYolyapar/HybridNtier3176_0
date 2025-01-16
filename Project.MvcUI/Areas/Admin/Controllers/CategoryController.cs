using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Project.Bll.DtoClasses;
using Project.Bll.Managers.Abstracts;
using Project.MvcUI.Areas.Admin.Models.PureVms.ResponseModels;

namespace Project.MvcUI.Areas.Admin.Controllers
{
    [Area("Admin")]
    //[Authorize(Roles ="Admin")]
    public class CategoryController : Controller
    {
        readonly ICategoryManager _catManager;
        readonly IMapper _mapper;

        public CategoryController(ICategoryManager catManager, IMapper mapper)
        {
            _mapper = mapper;
            _catManager = catManager;
        }

        public async Task<IActionResult> Index()
        {
            List<CategoryDto> categoryDtos = await _catManager.GetAllAsync();
            List<CategoryResponseModel> categories = _mapper.Map<List<CategoryResponseModel>>(categoryDtos);

            //Refactor burada PageVm icerisinde categories gönderilir
            return View(categories);
        }
    }
}
