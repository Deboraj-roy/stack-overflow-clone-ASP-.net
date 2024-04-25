using Autofac;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.WebUtilities;
using Stackoverflow.Infrastructure.Membership;
using System.ComponentModel.DataAnnotations;
using System.Text.Encodings.Web;
using System.Text;
using Stackoverflow.Application.Utilities;

namespace Stackoverflow.Web.Models
{
    public class EmailConfirmationModel
    {
        private ILifetimeScope _scope; 

        [Required]
        [EmailAddress]
        [Display(Name = "Email")]
        public string Email { get; set; }
         
        public string? ReturnUrl { get; set; }

        [Required]
        public string? Captcha { get; set; }

        public EmailConfirmationModel() { }
         
         
        internal async Task SendMailAsync( IEmailService emailService, UserManager<ApplicationUser> userManager, string urlPrefix)
        {
            ReturnUrl ??= urlPrefix;

            var user = await userManager.FindByEmailAsync(Email);
            if (user != null)
            {

                // Generate a new email confirmation code
                var code = await userManager.GenerateEmailConfirmationTokenAsync(user);
                
                // Encode the code
                code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));

                // Build the confirmation link
                var callbackUrl = $"{urlPrefix}/Account/ConfirmEmail?userId={user.Id}&code={code}&returnUrl={ReturnUrl}";

                // Send the confirmation email to the user
                emailService.SendSingleEmail($"Dear user {user.LastName} {user.LastName}", Email, "Confirm your email",
                    $"Please confirm your account by <a href='{HtmlEncoder.Default.Encode(callbackUrl)}'>clicking here</a>.");
                 
            }

        }

        internal void Resolve(ILifetimeScope scope)
        {
            _scope = scope; 
        }
    }
}
