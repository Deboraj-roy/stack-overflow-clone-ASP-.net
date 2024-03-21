using Autofac;
using Microsoft.AspNetCore.Mvc;

namespace Stackoverflow.Web.Areas.User.Controllers
{
    [Area("ApplicationUser")]
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
    }
}
