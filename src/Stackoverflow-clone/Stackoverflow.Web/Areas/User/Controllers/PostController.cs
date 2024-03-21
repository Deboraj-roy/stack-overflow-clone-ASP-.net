using Autofac;
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

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Create()
        {
            var model = _scope.Resolve<PostCreateModel>();
            return View(model);
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


    }
}
