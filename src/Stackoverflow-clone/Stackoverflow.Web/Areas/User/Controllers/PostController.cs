﻿using Autofac;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Org.BouncyCastle.Asn1.Ocsp;
using Stackoverflow.Domain.Exceptions;
using Stackoverflow.Infrastructure;
using Stackoverflow.Web.Areas.User.Models;

namespace Stackoverflow.Web.Areas.User.Controllers
{
    [Area("User")]
    public class PostController : Controller
    {
        private readonly ILifetimeScope _scope;
        private readonly ILogger<PostController> _logger;

        public PostController(ILifetimeScope scope,
            ILogger<PostController> logger)
        {
            _scope = scope;
            _logger = logger;
        }

        public async Task<IActionResult> Index(int page = 1)
        {
            int pageSize = 10; // Number of posts per page
            
            // Calculate the number of posts to skip based on the page number
            int skip = (page - 1) * pageSize;

            // Fetch only the posts for the current page

            var model = _scope.Resolve<PostListModel>();

            var posts = await model.GetPostsAsync();

            // Pass posts to your view
            var pagedPosts = posts.Skip(skip).Take(pageSize).ToArray();

            // Pass pagedPosts and other pagination information to your view
            ViewBag.PageNumber = page;
            ViewBag.TotalPages = (int)Math.Ceiling((double)posts.Length / pageSize);
            return View(pagedPosts);

        }

        public IActionResult Create()
        {
            var model = _scope.Resolve<PostCreateModel>();
            return View(model);
        }

        public async Task<IActionResult> Details(Guid id)
        {
            var model = _scope.Resolve<PostDetailsModel>();

            var post = await model.GetPostsDetailsAsync(id);

            if (post == null)
            {
                return NotFound();
            }

            return View(post);
        }

        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(PostCreateModel model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    model.Resolve(_scope);
                    await model.CreatePostAsync();

                    TempData["success"] = "Course created successfuly ";
                    _logger.LogInformation("Course created successfuly");
                    

                    return RedirectToAction("Index");
                } 
                catch (Exception de)
                {
                    _logger.LogError(de, "Server Error");

                    TempData["warning"] = de.Message + "There was a problem in creating course";
                     
                }
            }

            return View(model);
        }

        [HttpDelete]
        public async Task<IActionResult> Delete(Guid id)
        {
            // Delete the post with the specified ID
            // You need to implement the delete logic in your PostService
            var model = _scope.Resolve<PostDeleteModel>();

            await model.DeletePostAsync(id);
            TempData["warning"] = "Your Post deleted successfuly ";
            return Json(new { success = true, message = "Delete Successful" });
            //return RedirectToAction(nameof(Index));
        }

    }
}
