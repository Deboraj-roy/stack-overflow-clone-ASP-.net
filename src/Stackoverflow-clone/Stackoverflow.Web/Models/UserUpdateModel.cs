using Autofac;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.WebUtilities;
using Stackoverflow.Infrastructure.Membership;
using System.ComponentModel.DataAnnotations;
using System.Text.Encodings.Web;
using System.Text;
using Stackoverflow.Application.Utilities;
using Microsoft.AspNetCore.Mvc;
using AutoMapper;
using Stackoverflow.Domain.Entities;
using static System.Formats.Asn1.AsnWriter;

namespace Stackoverflow.Web.Models
{
    public class UserUpdateModel
    {
        private ILifetimeScope _scope;
        private UserManager<ApplicationUser> _userManager;
        private SignInManager<ApplicationUser> _signInManager;
        private IEmailService _emailService;
        private IMapper _mapper;


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

        public string? ProfilePicture { get; set; }

        // Updated to store the file path on the server
        internal string ProfilePicturePath { get; set; }

        // New property to handle file upload
        [Display(Name = "Profile Picture")]
        internal IFormFile ProfilePictureFile { get; set; }

        [Required]
        public string Captcha { get; set; }

        public UserUpdateModel() { }

        public UserUpdateModel(
            //UserManager<ApplicationUser> userManager,
            //SignInManager<ApplicationUser> signInManager,
            //IEmailService emailService,
            IMapper mapper
            )
        {
            //    _userManager = userManager;
            //    _signInManager = signInManager;
            //    _emailService = emailService;
            _mapper = mapper;
        }

        public async Task<bool> UpdateProfileAsync(UserManager<ApplicationUser> userManager2, string uploadPath)
        {
            if (!string.IsNullOrEmpty(ProfilePictureFile?.FileName))
            {
                string extension = Path.GetExtension(ProfilePictureFile.FileName);
                string fileName = $"{Guid.NewGuid()}" +"."+ $"{extension}";

                string filePath = Path.Combine(uploadPath, fileName);

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await ProfilePictureFile.CopyToAsync(stream);
                }

                ProfilePicturePath = filePath;
            }

            var user = await userManager2.FindByEmailAsync(Email);
            if (user != null)
            {
                user.FirstName = FirstName;
                user.LastName = LastName;
                user.PhoneNumber = PhoneNumber;

                if (!string.IsNullOrEmpty(ProfilePicturePath))
                {
                    user.ProfilePicture = ProfilePicturePath;
                }

                var result = await userManager2.UpdateAsync(user);
                return result.Succeeded;
            }

            return false;
        }

        internal async Task LoadAsync(string userId)
        {
            if (_mapper == null)
            {
                throw new Exception("Mapper is not initialized");
            }
            var user = await _userManager.FindByIdAsync(userId);
            if (user != null)
            {
               _mapper.Map(user, this);
            }
            //return user;
            //Post post = await _postService.GetPostAsync(id);
            //if (post != null)
            //{
            //    _mapper.Map(post, this);
            //}
        }

        internal void Resolve(ILifetimeScope scope)
        {
            _scope = scope;
            _userManager = _scope.Resolve<UserManager<ApplicationUser>>();
            _signInManager = _scope.Resolve<SignInManager<ApplicationUser>>();
            _emailService = _scope.Resolve<IEmailService>();
        }
         
    }
}