using Microsoft.AspNetCore.Mvc;

namespace Stackoverflow.Web.Areas.User.Controllers
{
    public class CommentController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
