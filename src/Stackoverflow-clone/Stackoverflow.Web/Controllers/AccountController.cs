using Autofac;
using GoogleReCaptcha.V3.Interface;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.WebUtilities;
using Stackoverflow.Infrastructure.Membership;
using Stackoverflow.Web.Models;
using System.Drawing;
using System.Text;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Processing;
using SixLabors.ImageSharp.PixelFormats;
using System.IO;
using Stackoverflow.Domain.Exceptions;
using Stackoverflow.Web.Areas.User.Models;
using Microsoft.AspNetCore.Identity.UI.Services;
using Stackoverflow.Application.Utilities;


namespace Stackoverflow.Web.Controllers
{
    public class AccountController : Controller
    {

        private readonly ILifetimeScope _scope;
        private readonly ILogger<AccountController> _logger;
        private readonly RoleManager<ApplicationRole> _roleManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly UserManager<ApplicationUser> _userManager;
        private IEmailService _emailService;
        private readonly ITokenService _tokenService;
        private readonly IConfiguration _configuration;
        private readonly ICaptchaValidator _captchaValidator;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public AccountController(ILifetimeScope scope,
            ILogger<AccountController> logger,
            RoleManager<ApplicationRole> roleManager,
            SignInManager<ApplicationUser> signInManager,
            UserManager<ApplicationUser> userManager,
            ITokenService tokenService,
            IConfiguration configuration,
            ICaptchaValidator captchaValidator,
            IEmailService emailService,
            IWebHostEnvironment webHostEnvironment)
        {
            _scope = scope;
            _logger = logger;
            _roleManager = roleManager;
            _signInManager = signInManager;
            _userManager = userManager;
            _tokenService = tokenService;
            _configuration = configuration;
            _captchaValidator = captchaValidator;
            _emailService = emailService;
            _webHostEnvironment = webHostEnvironment;
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
            //returnUrl ??= Url.Content("~/");

            // Check if the user is already authenticated
            if (User.Identity.IsAuthenticated)
            {
                await _signInManager.SignOutAsync(); // Log out the user
                //recomended to clear the session
                HttpContext.Session.Remove("token");
                HttpContext.Session.Clear();
            }
            var model = _scope.Resolve<LoginModel>();

            // Clear the existing external cookie to ensure a clean login process
            await HttpContext.SignOutAsync(IdentityConstants.ExternalScheme);

            if (!string.IsNullOrEmpty(returnUrl))
            {
                // Decode and set the return URL
                model.ReturnUrl = Uri.UnescapeDataString(returnUrl);
            }
            else
            {
                model.ReturnUrl = Url.Content("~/");
            }

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
                    var user = await _userManager.FindByEmailAsync(model.Email);
                    var claims = (await _userManager.GetClaimsAsync(user)).ToArray();
                    var token = await _tokenService.GetJwtToken(claims,
                            _configuration["Jwt:Key"],
                            _configuration["Jwt:Issuer"],
                            _configuration["Jwt:Audience"]
                        );
                    HttpContext.Session.SetString("token", token);

                    _logger.LogInformation("Login Successfully");
                    TempData["success"] = "Login Successfully ";
                    // Check if the returnUrl is a valid URL
                    if (!string.IsNullOrEmpty(model.ReturnUrl) && Url.IsLocalUrl(model.ReturnUrl))
                    {
                        return Redirect(model.ReturnUrl);
                    }
                    else
                    {
                        return RedirectToAction("Index", "Post", new { area = "User" });
                    }
                }
                else
                {
                    _logger.LogInformation("Invalid login attempt.");
                    ModelState.AddModelError(string.Empty, "Invalid login attempt.");
                    return View(model);
                }

            }

            // If we got this far, something failed, redisplay form
            return View(model);
        }

        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> LogoutAsync(string returnUrl = null)
        {
            await _signInManager.SignOutAsync();
            //recomended to clear the session
            HttpContext.Session.Remove("token");
            //HttpContext.Session.Clear();

            _logger.LogInformation("LogOut Successfully.");
            TempData["warning"] = "LogOut Successfully ";
            if (returnUrl != null)
            {
                return LocalRedirect(returnUrl);
            }
            else
            {
                return RedirectToAction("Index", "Post", new { area = "User" });
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
                //return Redirect(returnUrl ?? "/"); // You may adjust the default page as needed
                return RedirectToAction("Index", "Post", new { area = "User" });
            }
            else
            {
                // Failed to confirm email
                // You may handle the failure appropriately, such as showing an error message
                return BadRequest("Failed to confirm email.");
            }
        }

        public IActionResult ResendEmailConfirmation()
        {
            var model = _scope.Resolve<EmailConfirmationModel>();
            return View(model);
        }

        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> ResendEmailConfirmation(EmailConfirmationModel model)
        {
            if (ModelState.IsValid)
            {
                var baseUrl = $"{Request.Scheme}://{Request.Host}";


                await model.SendMailAsync(_emailService, _userManager, baseUrl);

                TempData["success"] = "Email confirmation link has been sent successfully, check your email.";
                return RedirectToAction("Index", "Post", new { area = "User" });
            }
            else
            {
                return View(model);
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


        public async Task<IActionResult> Update(string userId)
        {

            var model = _scope.Resolve<UserUpdateModel>();

            await model.LoadAsync(_userManager, userId);

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Update(UserUpdateModel model)
        {

            if (ModelState.IsValid)
            {
                if (model.ProfilePictureFile is not null && model.ProfilePictureFile.Length > 0)
                {
                    try
                    {
                        using (var image = SixLabors.ImageSharp.Image.Load<Rgba32>(model.ProfilePictureFile.OpenReadStream()))
                        {

                            if (!model.IsImage(model.ProfilePictureFile))
                            {
                                _logger.LogInformation("Invalid image file type. Please upload a valid image file.");
                                ModelState.AddModelError("ProfilePictureFile", "Please upload a valid image file.");
                                return View(model);
                            }
                        }
                    }
                    catch
                    {
                        _logger.LogInformation("An error occurred while processing the uploaded file.");
                        ModelState.AddModelError("ProfilePictureFile", "An error occurred while processing the uploaded file.");
                        return View(model);
                    }

                    string wwwRootPath = _webHostEnvironment.WebRootPath;
                    //string fileName = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);
                    string productPath = @"Files";
                    string uploadPath = Path.Combine(wwwRootPath, productPath);

                    //string uploadPath = Path.Combine("~/", "files");
                    bool updateResult = await model.UpdateProfileAsync(_userManager, uploadPath);

                    if (updateResult)
                    {
                        _logger.LogInformation("User profile updated successfully.");
                        TempData["success"] = "User profile updated successfully.";
                        return RedirectToAction("Index", "Post", new { area = "User" });

                    }
                    else
                    {
                        _logger.LogInformation("Failed to update user profile.");
                        TempData["error"] = "Failed to update user profile.";
                        ModelState.AddModelError(string.Empty, "Failed to update user profile.");
                        return View(model);

                    }

                }
                else
                {
                    ModelState.AddModelError("ProfilePictureFile", "Please select a file to upload.");
                    return View(model);
                }
            }
            return View(model);
        }


        //only for admin
        [Authorize(Roles = UserRoles.Admin)]
        public async Task<IActionResult> CreateRoles()
        {
            await _roleManager.CreateAsync(new ApplicationRole { Name = UserRoles.Admin });
            await _roleManager.CreateAsync(new ApplicationRole { Name = UserRoles.User });
            await _roleManager.CreateAsync(new ApplicationRole { Name = UserRoles.Newbie });
            await _roleManager.CreateAsync(new ApplicationRole { Name = UserRoles.Elite });
            await _roleManager.CreateAsync(new ApplicationRole { Name = UserRoles.PowerUser });
            await _roleManager.CreateAsync(new ApplicationRole { Name = UserRoles.VIP });

            return View();
        }
    }
}