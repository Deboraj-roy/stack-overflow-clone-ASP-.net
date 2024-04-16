using Autofac;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Org.BouncyCastle.Asn1.Ocsp;
using Stackoverflow.Domain.Entities;
using Stackoverflow.Domain.Exceptions;
using Stackoverflow.Infrastructure;
using Stackoverflow.Infrastructure.Membership;
using Stackoverflow.Web.Areas.User.Models;
using System.Net;
using System.Net.Http;

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

        [AllowAnonymous] 
        public async Task<IActionResult> Index(int page = 1)
        {
            int pageSize = 10; // Number of posts per page
            
            // Calculate the number of posts to skip based on the page number
            int skip = (page - 1) * pageSize;

            // Fetch only the posts for the current page

            var model = _scope.Resolve<PostListModel>();

            try
            {
                var token = HttpContext.Session.GetString("token");
                var posts = await model.GetPostsAsync(token);

                // Process the fetched posts
                //var posts = await model.GetPostsAsync();

                // Pass posts to your view
                var pagedPosts = posts.Skip(skip).Take(pageSize).ToArray();

                // Pass pagedPosts and other pagination information to your view
                ViewBag.PageNumber = page;
                ViewBag.TotalPages = (int)Math.Ceiling((double)posts.Length / pageSize);
                return View(pagedPosts);
            }
            catch (HttpRequestException ex) when (ex.StatusCode == HttpStatusCode.Unauthorized)
            {
                // Handle 401 Unauthorized exception
                // Redirect to login page or display a message asking the user to login again


                // Set a TempData message to be displayed in the view
                TempData["ErrorMessage"] = "Your session has expired. Please login again.";

                // Option 1: Redirect to login page
                return RedirectToAction("Login", "Account", new { area = "" });

                // Option 2: Display a message asking the user to login again
                // Set a TempData message to be displayed in the view
                //TempData["ErrorMessage"] = "Your session has expired. Please login again.";

                // Redirect to a specific action that displays the message
                //return RedirectToAction("Index");
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
        public async Task<IActionResult> Create(PostCreateModel model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    model.Resolve(_scope);
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
                return NotFound();
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

        //public async Task<IActionResult> Search(string searchTerm)
        //{
        //    var model = _scope.Resolve<PostListModel>();

        //    //await model.GetPostsSearchAPIAsync(searchTerm);
        //    await model.GetPagedPostsAsync(searchTerm);

        //    return View(model);
        //}
        //[HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> Search(string searchString)
        {
            ViewBag.SearchString = searchString;
            var model = _scope.Resolve<PostSearchModel>();
            model.Title = searchString;
            var post = await model.GetPostsSearchAPIAsync();

            if (post == null)
            {
                return NotFound();
            }

            return View(post);

            //return View();
        }

    }
}
