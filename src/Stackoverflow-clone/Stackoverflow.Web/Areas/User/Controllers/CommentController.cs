using Autofac;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Stackoverflow.Domain.Entities;
using Stackoverflow.Web.Areas.User.Models;
using System.Security.Claims;
using static System.Formats.Asn1.AsnWriter;

namespace Stackoverflow.Web.Areas.User.Controllers
{
    [Authorize]
    public class CommentController : Controller
    {
        private readonly ILifetimeScope _scope;
        private readonly ILogger<CommentController> _logger;

        public CommentController(ILifetimeScope scope,
            ILogger<CommentController> logger)
        {
            _scope = scope;
            _logger = logger;
        }
        public IActionResult Index()
        {
            return View();
        }

        //[Authorize(Policy = "CommentCreatePolicy")]
        public async Task<IActionResult> CreateAsync(Guid postId, CommentCreateModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            //var model2 = _scope.Resolve<CommentCreateModel>();


            // Get the current user's ID
            model.userId = Guid.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);
            //model2.Body = model.Body;
            model.postId = postId;
            await model.CreateCommentAsync();
            // Create a new comment entity
            //var comment = new Comment
            //{
            //    Id = Guid.NewGuid(),
            //    UserId = Guid.Parse(userId),
            //    PostId = postId,
            //    Body = model.Body,
            //    CreationDate = DateTime.UtcNow,
            //    LastModifiedDate = DateTime.UtcNow,
            //    IsDeleted = false
            //};

            // Save the new comment to the database
            // You can use your repository or service layer to save the comment
            // For example:
            // _commentRepository.Add(comment);
            //model.CreateCommentAsync();
            // _commentRepository.SaveChanges();

            // Redirect back to the post details page
            return RedirectToAction("Details", "Post", new { id = postId });
        }
    }
}
