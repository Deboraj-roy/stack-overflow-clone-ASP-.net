﻿using Autofac;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Stackoverflow.Domain.Exceptions;
using Stackoverflow.Web.Areas.User.Models;
using System.Net;

namespace Stackoverflow.Web.Areas.User.Controllers
{
    [Area("User")]
    [Authorize]
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

        [Authorize] 
        public async Task<IActionResult> Index(int page = 1)
        {
            int pageSize = 10; // Number of posts per page
            
            // Calculate the number of posts to skip based on the page number
            int skip = (page - 1) * pageSize;

            // Fetch only the posts for the current page

            var model = _scope.Resolve<PostListModel>();

            try
            {
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;

                var token = HttpContext.Session.GetString("token");
                var posts = await model.GetPostsAsync(token);
                if (posts == null)
                {
                   return View("NotFoundPartial");
                }
                // Pass posts to your view
                var pagedPosts = posts.Skip(skip).Take(pageSize).ToArray();

                // Pass pagedPosts and other pagination information to your view
                ViewBag.PageNumber = page;
                ViewBag.TotalPages = (int)Math.Ceiling((double)posts.Length / pageSize);
                return View(pagedPosts);
            }
            catch (HttpRequestException ex) when (ex.StatusCode == HttpStatusCode.Unauthorized)
            {
                // Set a TempData message to be displayed in the view
                TempData["ErrorMessage"] = "Your session has expired. Please login again.";

                // Option 1: Redirect to login page
                return RedirectToAction("Login", "Account", new { area = "" });
            }


        }
         
        [Authorize(Policy = "PostCreatePolicy")]
        public IActionResult Create()
        {
            var model = _scope.Resolve<PostCreateModel>();
            return View(model);
        }

        [HttpPost, ValidateAntiForgeryToken]
        [Authorize(Policy = "PostCreatePolicy")]
        public async Task<IActionResult> Create(PostCreateModel model, Guid userId)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    model.Resolve(_scope);
                    model.userId = userId;
                    await model.CreatePostAsync();

                    TempData["success"] = "Post created successfuly ";
                    _logger.LogInformation("Post created successfuly");


                    return RedirectToAction("Index");
                }
                catch (Exception de)
                {
                    _logger.LogError(de, "Server Error");

                    TempData["warning"] = de.Message + "There was a problem in creating Post";

                }
            }

            return View(model);
        }
         
        //[Authorize(Policy = "PostViewPolicy")]
        [Authorize(Policy = "PostViewRequirementPolicy")]
        public async Task<IActionResult> Details(Guid id)
        {
            var model = _scope.Resolve<PostDetailsModel>();

            var post = await model.GetPostsDetailsAsync(id);

            if (post == null)
            {
                  return View("NotFoundPartial");
            }

            return View(post);
        }
         

        [HttpDelete, Authorize(Policy = "SupperAdmin")]
        public async Task<IActionResult> Delete(Guid id)
        {
            // Delete the post with the specified ID
            // You need to implement the delete logic in your PostService
            var model = _scope.Resolve<PostDeleteModel>();

            await model.DeletePostAsyncAPI(id);
            TempData["warning"] = "Your Post deleted successfuly ";
            return Json(new { success = true, message = "Delete Successful" });
            //return RedirectToAction(nameof(Index));
        }

        [Authorize(Policy = "PostUpdatePolicy")]
        public async Task<IActionResult> Update(Guid id)
        {
            var model = _scope.Resolve<PostUpdateModel>();

            await model.LoadAsync(id);
             
            return View(model);
        }
         
        [HttpPost, ValidateAntiForgeryToken]
        [Authorize(Policy = "PostUpdatePolicy")]
        public async Task<IActionResult> Update(PostUpdateModel model)
        {
            model.Resolve(_scope);

            if (ModelState.IsValid)
            {
                try
                {
                    await model.UpdatePostAsync(); 
                    TempData["success"] = "Your Post updated successfuly ";
                    return RedirectToAction("Index");
                }
                catch (DuplicateTitleException de)
                {
                    TempData["warning"] = de.Message;
                }
                catch (Exception e)
                {
                    _logger.LogError(e, "Server Error");

                    TempData["warning"] = "There was a problem in updating Post ";
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

        [AllowAnonymous]
        public async Task<IActionResult> Search(string searchString)
        {
            ViewBag.SearchString = searchString;
            var model = _scope.Resolve<PostSearchModel>();
            model.Title = searchString;
            var post = await model.GetPostsSearchAPIAsync();

            if (post == null)
            {
                  return View("NotFoundPartial");
            }

            return View(post);
        }

    }
}
