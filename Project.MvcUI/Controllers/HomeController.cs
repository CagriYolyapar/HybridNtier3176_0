using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.Metadata;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Project.Bll.Managers.Abstracts;
using Project.Common.Tools;
using Project.Entities.Models;
using Project.MvcUI.Models;
using Project.MvcUI.Models.PureVms.AppUsers;
using System.Diagnostics;
using SignInResult = Microsoft.AspNetCore.Identity.SignInResult;

namespace Project.MvcUI.Controllers
{
    public class HomeController : Controller
    {

        private readonly ILogger<HomeController> _logger;
        readonly UserManager<AppUser> _userManager;
        readonly SignInManager<AppUser> _signInManager;
        readonly RoleManager<IdentityRole<int>> _roleManager;
        readonly IMapper _mapper;



        public HomeController(ILogger<HomeController> logger, UserManager<AppUser> userManager, SignInManager<AppUser> signInManager, RoleManager<IdentityRole<int>> roleManager, IMapper mapper)
        {
            _logger = logger;
            _userManager = userManager;
            _signInManager = signInManager;
            _roleManager = roleManager;
            _mapper = mapper;
        }

        public IActionResult Index()
        {
            return View();
        }

        [Authorize(Roles = "Member")]
        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Register(UserRegisterRequestModel model)
        {
            Guid specId = Guid.NewGuid();
            AppUser appUser = new()
            {
                UserName = model.UserName,
                Email = model.Email,
                ActivationCode = specId
            };

            IdentityResult result = await _userManager.CreateAsync(appUser, model.Password);

            if (result.Succeeded)
            {
                #region RolKontrolIslemleri

                //Todo: İş akısı refactor'u
                IdentityRole<int> appRole = await _roleManager.FindByNameAsync("Member");
                if (appRole == null) await _roleManager.CreateAsync(new() { Name = "Member" });
                //Her halükarda elimize Member rolü gececek
                await _userManager.AddToRoleAsync(appUser, "Member");
                #endregion

                string message = $"Hesabınız olusturulmustur...Üyeliginizi onaylamak icin lütfen http://localhost:5172/Home/ConfirmEmail?specId={specId}&id={appUser.Id} linkine tıklayınız ";

                MailService.Send(model.Email, body: message);
                TempData["Message"] = "Lütfen hesabınızı onaylamak icin emailinizi kontrol ediniz";

                return RedirectToAction("RedirectPanel");
            }

            return View();

        }

        public IActionResult RedirectPanel()
        {
            return View();
        }

        public async Task<IActionResult> ConfirmEmail(Guid specId, int id)
        {
            AppUser user = await _userManager.FindByIdAsync(id.ToString());
            if (user == null)
            {
                TempData["Message"] = "Kullanıcı bulunamadı";
                return RedirectToAction("RedirectPanel");
            }
            else if (user.ActivationCode == specId)
            {
                user.EmailConfirmed = true;
                await _userManager.UpdateAsync(user);
                TempData["Message"] = "Hesabınız basarıyla onaylandı";
                return RedirectToAction("SignIn");
            }

            //Loglama iş akısı
            return RedirectToAction("Register");
        }

        public IActionResult SignIn()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> SignIn(UserRegisterRequestModel model)
        {
            AppUser appUser = await _userManager.FindByNameAsync(model.UserName);
            SignInResult result = await _signInManager.PasswordSignInAsync(appUser, model.Password, true, true);

            if (result.Succeeded)
            {
                IList<string> roles = await _userManager.GetRolesAsync(appUser);
                if (roles.Contains("Admin"))
                {
                    return RedirectToAction("Index", "Category", new { Area = "Admin" });
                }
                else if (roles.Contains("Member"))
                {
                    return RedirectToAction("Privacy");
                }
                return RedirectToAction("Index");
            }
            else if (result.IsNotAllowed)
            {
                return RedirectToAction("MailPanel");
            }

            TempData["Message"] = "Kullanıcı bulunamadı";
            return RedirectToAction("SignIn");

         
        }

        public IActionResult MailPanel()
        {
            return View();
        }
    }
}
