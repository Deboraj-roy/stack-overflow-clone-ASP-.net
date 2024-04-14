using Autofac;
using GoogleReCaptcha.V3.Interface;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.WebUtilities;
using Stackoverflow.Domain.Exceptions;
using Stackoverflow.Infrastructure.Membership;
using Stackoverflow.Web.Areas.User.Models;
using Stackoverflow.Web.Models;
using System.Text;

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
        private readonly ICaptchaValidator _captchaValidator;

        public AccountController(ILifetimeScope scope,
            ILogger<AccountController> logger,
            RoleManager<ApplicationRole> roleManager,
            SignInManager<ApplicationUser> signInManager,
            UserManager<ApplicationUser> userManager,
            //ITokenService tokenService,
            IConfiguration configuration,
            ICaptchaValidator captchaValidator)
        {
            _scope = scope;
            _logger = logger;
            _roleManager = roleManager;
            _signInManager = signInManager;
            _userManager = userManager;
            //_tokenService = tokenService;
            _configuration = configuration;
            _captchaValidator = captchaValidator;
        }

        public IActionResult Register()
        {
            var model = _scope.Resolve<RegistrationModel>();
            return View(model);
        }

        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(RegistrationModel model)
        {
            //if (!await _captchaValidator.IsCaptchaPassedAsync(model.Captcha))
            //{
            //    await Console.Out.WriteLineAsync("Invalid captcha");
            //}
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
                    _logger.LogInformation("User Register Failed");
                    return View(model);

                }
                else
                    TempData["success"] = "User Registered Successfully ";
                    _logger.LogInformation("User Registered Successfully");
                    return Redirect(response.redirectLocation);
            }
            return View(model);


        }

        public async Task<IActionResult> LoginAsync(string returnUrl = null)
        {
            returnUrl ??= Url.Content("~/");

            var model = _scope.Resolve<LoginModel>();

            // Clear the existing external cookie to ensure a clean login process
            await HttpContext.SignOutAsync(IdentityConstants.ExternalScheme);

            model.ReturnUrl = returnUrl;
            return View(model);
        }

        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> LoginAsync(LoginModel model)
        {
            model.ReturnUrl ??= Url.Content("~/");

            if (ModelState.IsValid)
            {
                // This doesn't count login failures towards account lockout
                // To enable password failures to trigger account lockout, set lockoutOnFailure: true
                var result = await _signInManager.PasswordSignInAsync(model.Email, model.Password, model.RememberMe, lockoutOnFailure: false);
                if (result.Succeeded)
                {
                    _logger.LogInformation("Login Successfully");
                    TempData["success"] = "Login Successfully ";
                    return LocalRedirect(model.ReturnUrl);
                }
                else
                {
                    _logger.LogInformation("Invalid login attempt.");
                    ModelState.AddModelError(string.Empty, "Invalid login attempt.");
                }

            }

            // If we got this far, something failed, redisplay form
            return View(model);
        }

        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> LogoutAsync(string returnUrl = null)
        {
            await _signInManager.SignOutAsync();

            _logger.LogInformation("LogOut Successfully.");
            TempData["warning"] = "LogOut Successfully ";
            if (returnUrl != null)
            {
                return LocalRedirect(returnUrl);
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
        }

        public async Task<IActionResult> ConfirmEmail(string userId, string code, string returnUrl)
        {
            if (userId == null || code == null)
            {
                // Handle invalid parameters
                return BadRequest("User Id or code is missing.");
            }

            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                // Handle user not found
                return NotFound("User not found.");
            }

            // Decode the code
            code = Encoding.UTF8.GetString(WebEncoders.Base64UrlDecode(code));

            // Confirm the user's email
            var result = await _userManager.ConfirmEmailAsync(user, code);
            if (result.Succeeded)
            {  
                TempData["success"] = "Email confirmed successfully ";
                // Email confirmed successfully
                // Redirect to the specified returnUrl or a default page
                return Redirect(returnUrl ?? "/"); // You may adjust the default page as needed
            }
            else
            {
                // Failed to confirm email
                // You may handle the failure appropriately, such as showing an error message
                return BadRequest("Failed to confirm email.");
            }
        }

        
        public async Task<IActionResult> Manage(string userId)
        {
            if (string.IsNullOrEmpty(userId))
            {
                return NotFound();
            }

            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                return NotFound();
            }

            return View(user);
        }

        public async Task<IActionResult> Update(Guid id)
        {
            var model = _scope.Resolve<UserUpdateModel>();

            return View(model);
        }

        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> Update(UserUpdateModel model)
        {
            model.Resolve(_scope);

            if (ModelState.IsValid)
            {
                try
                {
                    await model.UpdateCourseAsync();
                    return RedirectToAction("Index");
                }
                catch (DuplicateTitleException de)
                {
                    TempData["warning"] = de.Message;
                }
                catch (Exception e)
                {
                    _logger.LogError(e, "Server Error");


                    TempData["warning"] = "There was a problem in updating course ";
                }
            }
            else
            {

                foreach (var modelStateEntry in ModelState.Values)
                {
                    foreach (var error in modelStateEntry.Errors)
                    {
                        _logger.LogError($"Validation error: {error.ErrorMessage}");
                    }
                }

            }

            return View(model);
        }


        [HttpPost]
        public async Task<IActionResult> UploadProfilePicture(string userId, IFormFile file)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                return NotFound();
            }

            // Handle file upload logic and update user profile picture property

            return RedirectToAction(nameof(Manage), new { userId });
        }


        //only for admin
        [Authorize(Roles = UserRoles.Admin)]
        public async Task<IActionResult> CreateRoles()
        {
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
