﻿using Autofac;
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

        [Required]
        [EmailAddress]
        [Display(Name = "Email")]
        public string Email { get; set; }

        [Required]
        [Display(Name = "First Name")]
        public string? FirstName { get; set; }

        [Required]
        [Display(Name = "Last Name")]
        public string? LastName { get; set; }

        public string? PhoneNumber { get; set; }

        public string? ProfilePicture { get; set; }

        // New property to handle file upload
        [Display(Name = "Profile Picture")]
        public IFormFile ProfilePictureFile { get; set; }

        [Required]
        public string Captcha { get; set; }

        public UserUpdateModel() { }

        public async Task<bool> UpdateProfileAsync(UserManager<ApplicationUser> userManager2, string uploadPath)
        {
            if (!string.IsNullOrEmpty(ProfilePictureFile?.FileName))
            {
                string extension = Path.GetExtension(ProfilePictureFile.FileName);
                string fileName = $"{Guid.NewGuid()}{extension}";
                string filePath = Path.Combine(uploadPath, fileName);

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await ProfilePictureFile.CopyToAsync(stream);
                }

                //ProfilePicturePath = filePath;
                ProfilePicture = $"/files/{fileName}";
            }

            var user = await userManager2.FindByEmailAsync(Email);
            if (user != null)
            {
                user.FirstName = FirstName;
                user.LastName = LastName;
                user.PhoneNumber = PhoneNumber;

                if (!string.IsNullOrEmpty(ProfilePicture))
                {
                    user.ProfilePicture = ProfilePicture;
                }

                var result = await userManager2.UpdateAsync(user);
                return result.Succeeded;
            }

            return false;
        }

        internal async Task LoadAsync(UserManager<ApplicationUser> userManager1, string userId)
        {
            var user = await userManager1.FindByIdAsync(userId);
            if (user != null)
            {
                Email = user.Email;
                FirstName = user.FirstName;
                LastName = user.LastName;
                PhoneNumber = user.PhoneNumber;
                ProfilePicture = user.ProfilePicture;
            }
        }

        internal void Resolve(ILifetimeScope scope)
        {
            _scope = scope;
        }

    }
}