using Autofac;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.WebUtilities;
using Stackoverflow.Infrastructure.Membership;
using System.ComponentModel.DataAnnotations;
using System.Text.Encodings.Web;
using System.Text;
using Stackoverflow.Application.Utilities;
using Microsoft.AspNetCore.Mvc;

namespace Stackoverflow.Web.Models
{
    public class UserUpdateModel
    {
        private ILifetimeScope _scope;
        private UserManager<ApplicationUser> _userManager;
        private SignInManager<ApplicationUser> _signInManager;
        private IEmailService _emailService;

        [Required]
        [EmailAddress]
        [Display(Name = "Email")]
        public string Email { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Confirm password")]
        [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; }

        [Required]
        [Display(Name = "First Name")]
        public string? FirstName { get; set; }
        
        [Required]
        [Display(Name = "Last Name")]
        public string? LastName { get; set; }

        public string? PhoneNumber { get; set; }

        public string? ProfilePicture { get; set; } = "https://cdn.pixabay.com/photo/2015/10/05/22/37/blank-profile-picture-973460_1280.png";
        
        [Required]
        public string Captcha { get; set; }

        public UserUpdateModel() { }

        public UserUpdateModel(UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager,
            IEmailService emailService)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _emailService = emailService;
        }

        //internal async Task<(IEnumerable<IdentityError>? errors, string? redirectLocation)> RegisterAsync(string urlPrefix)
        //{
        //    ReturnUrl ??= urlPrefix;

        //    var user = new ApplicationUser { 
        //        UserName = Email, Email = Email, FirstName = FirstName, LastName = LastName,
        //        PhoneNumber = PhoneNumber, ProfilePicture = ProfilePicture
        //    };
        //    var result = await _userManager.CreateAsync(user, Password);
        //    if (result.Succeeded)
        //    {
        //        await _userManager.AddToRoleAsync(user, UserRoles.Elite);
        //        await _userManager.AddToRoleAsync(user, UserRoles.Admin);
        //        //await _userManager.AddClaimAsync(user, new System.Security.Claims.Claim("UpdateCourse", "true"));
        //        //await _userManager.AddClaimAsync(user, new System.Security.Claims.Claim("ViewCourse", "true"));

        //        var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
        //        code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));
        //        var callbackUrl = $"{urlPrefix}/Account/ConfirmEmail?userId={user.Id}&code={code}&returnUrl={ReturnUrl}";

        //        _emailService.SendSingleEmail(FirstName + " " + LastName, Email, "Confirm your email",
        //            $"Please confirm your account by <a href='{HtmlEncoder.Default.Encode(callbackUrl)}'>clicking here</a>.");


        //        if (_userManager.Options.SignIn.RequireConfirmedAccount)
        //        {
        //            var confirmationPageLink = $"RegisterConfirmation?email={Email}&returnUrl={ReturnUrl}";
        //            return (null, confirmationPageLink);
        //        }
        //        else
        //        {
        //            await _signInManager.SignInAsync(user, isPersistent: false);
        //            return (null, ReturnUrl);
        //        }
        //    }
        //    else
        //    {
        //        return (result.Errors, null);
        //    }
        //}

        internal void Resolve(ILifetimeScope scope)
        {
            _scope = scope;
            _userManager = _scope.Resolve<UserManager<ApplicationUser>>();
            _signInManager = _scope.Resolve<SignInManager<ApplicationUser>>();
            _emailService = _scope.Resolve<IEmailService>();
        }
    }
}
