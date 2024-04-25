using Autofac;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Stackoverflow.API.RequestHandlers;
using Stackoverflow.Domain.Entities;

namespace Stackoverflow.API.Controllers
{
    [ApiController]
    [Route("v3/[controller]")]
    [EnableCors("AllowSites")]
    public class PostController : ControllerBase
    {
        private readonly ILifetimeScope _scope;
        private readonly ILogger<PostController> _logger;

        public PostController(ILogger<PostController> logger, ILifetimeScope scope)
        {
            _logger = logger;
            _scope = scope;
        }
         
        //[HttpPost]
        [HttpPost, Authorize(Policy = "PostViewRequirementPolicy")]
        public async Task<object> Post([FromBody] ViewPostRequestHandler handler)
        {
            handler.ResolveDependency(_scope);
            var data = await handler.GetPagedPosts();
            _logger.LogInformation("Posts found");
            return data;
        }

        //[HttpPost("search"), Authorize(Policy = "PostViewRequirementPolicy")]
        [HttpGet("search")]
        public async Task<IEnumerable<Post>> Search(string title)
        {
            var handler = _scope.Resolve<ViewPostRequestHandler>();
            handler.Title = title;
            var posts = await handler.GetPostsAsync();
            _logger.LogInformation("Posts found");
             
            var filteredPosts = posts.Where(p => p.Title.Contains(title)); // Filter posts based on the search query

            return filteredPosts;
        }


        //[HttpGet]
        [HttpGet, Authorize(Policy = "PostViewRequirementPolicy")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IEnumerable<Post>> Get()
        {
            try
            {
                var model = _scope.Resolve<ViewPostRequestHandler>();

                _logger.LogInformation("All Posts found");
                var posts = await model?.GetPostsAsync();

                if (posts != null)
                {
                    return (IEnumerable<Post>)Ok(posts);
                }
                else
                {
                    return (IEnumerable<Post>)NotFound("No posts found");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Couldn't get Posts");
                return (IEnumerable<Post>)Unauthorized(new { Message = ex.Message });
            }
        }

        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<Post> Get(Guid id)
        {
            var model = _scope.Resolve<ViewPostRequestHandler>();
            return await model?.GetPostAsync(id);
        }

        [HttpPost("Create")]
        //[ValidateAntiForgeryToken]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Create([FromBody] PostCreateModel model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    model.Resolve(_scope);
                    await model.CreatePostAsync();

                    _logger.LogInformation("Post created successfully");

                    return Ok(new { message = "Post created successfully" });


                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Server Error");
                    return StatusCode(500, new { error = "Internal Server Error", message = ex.Message });
                }
            }
            else
            {
                return BadRequest(ModelState);
            }
        }

        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Update(Guid id, [FromBody] PostUpdateModel model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    model.Resolve(_scope);
                    if (await model.UpdatePostAsync())
                    {
                        _logger.LogInformation("Post updated successfully");
                        return Ok();
                    }
                    else
                    {
                        return BadRequest();
                    }
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Server Error");
                    return StatusCode(500, new { error = "Internal Server Error", message = ex.Message });
                }
            }
            else
            {
                return BadRequest(ModelState);
            }
        }

        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Delete(Guid id)
        {
            try
            {
                var model = _scope.Resolve<ViewPostRequestHandler>();
                if (await model.DeletePost(id))
                {
                    _logger.LogInformation("Post deleted");
                    return NoContent();
                }
                else
                {
                    return BadRequest();
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Couldn't delete Post {id}");
                return BadRequest();
            }
        }

    }
}
