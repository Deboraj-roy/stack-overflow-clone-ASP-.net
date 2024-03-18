using Autofac;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Stackoverflow.Infrastructure.Membership;
using Stackoverflow.Web.Models;

namespace Stackoverflow.Web.Controllers
{
    public class AccountController : Controller
    {

        private readonly ILifetimeScope _scope;
        private readonly ILogger<AccountController> _logger;
        private readonly RoleManager<ApplicationRole> _roleManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly UserManager<ApplicationUser> _userManager;
        //private readonly ITokenService _tokenService;
        private readonly IConfiguration _configuration;

        public AccountController(ILifetimeScope scope,
            ILogger<AccountController> logger,
            RoleManager<ApplicationRole> roleManager,
            SignInManager<ApplicationUser> signInManager,
            UserManager<ApplicationUser> userManager,
            //ITokenService tokenService,
            IConfiguration configuration)
        {
            _scope = scope;
            _logger = logger;
            _roleManager = roleManager;
            _signInManager = signInManager;
            _userManager = userManager;
            //_tokenService = tokenService;
            _configuration = configuration;
        }

        public IActionResult Register()
        {
            var model = _scope.Resolve<RegistrationModel>();
            return View(model);
        }

        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(RegistrationModel model)
        {

            if (ModelState.IsValid)
            {
                model.Resolve(_scope);
                var baseUrl = $"{Request.Scheme}://{Request.Host}";

                // If you need to include the port number in the URL
                // var baseUrl = $"{Request.Scheme}://{Request.Host.Value}";
                //var response = await model.RegisterAsync(Url.Content("~/"));

                var response = await model.RegisterAsync(Url.Content(baseUrl));

                if (response.errors is not null)
                {
                    foreach (var error in response.errors)
                    {
                        ModelState.AddModelError(string.Empty, error.Description);
                    }
                    return View(model);
                }
                else
                    return Redirect(response.redirectLocation);
            }
            return View(model);


        }

        public async Task<IActionResult> CreateRoles()
        {
            //await _roleManager.CreateAsync(new ApplicationRole { Name = "Admin"});
            //await _roleManager.CreateAsync(new ApplicationRole { Name = "User" });
            //await _roleManager.CreateAsync(new ApplicationRole { Name = "Employee" });
            //await _roleManager.CreateAsync(new ApplicationRole { Name = "Supervisor" });
            await _roleManager.CreateAsync(new ApplicationRole { Name = UserRoles.Admin });
            await _roleManager.CreateAsync(new ApplicationRole { Name = UserRoles.User });
            await _roleManager.CreateAsync(new ApplicationRole { Name = UserRoles.Newbie });
            await _roleManager.CreateAsync(new ApplicationRole { Name = UserRoles.Elite});
            await _roleManager.CreateAsync(new ApplicationRole { Name = UserRoles.PowerUser });
            await _roleManager.CreateAsync(new ApplicationRole { Name = UserRoles.VIP });

            return View();
        }
    }
}
