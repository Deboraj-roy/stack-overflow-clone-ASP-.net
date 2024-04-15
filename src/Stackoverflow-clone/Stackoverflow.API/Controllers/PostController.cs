using Autofac;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Stackoverflow.Api.RequestHandlers;
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

        //[HttpPost("view")]
        [HttpPost, Authorize(Policy = "PostViewRequirementPolicy")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<object> Post([FromBody] ViewPostRequestHandler handler)
        {
            handler.ResolveDependency(_scope);
            var data = await handler.GetPagedPosts();
            _logger.LogInformation("Posts found");
            return data;
        }


        //[HttpGet]
        [HttpGet, Authorize(Policy = "PostViewRequirementPolicy")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IEnumerable<Post>> Get()
        {
            try
            {
                var model = _scope.Resolve<ViewPostRequestHandler>();

                _logger.LogInformation("All Posts found");
                return await model?.GetPostsAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Couldn't get Posts");
                return null;
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

        //[HttpGet("{name}")]
        //public Post Get(string name)
        //{
        //    var model = _scope.Resolve<PostModel>();
        //    return model.GetPost(name);
        //}

        //[HttpPost()]
        //public IActionResult Post([FromBody] ViewPostRequestHandler model)
        //{
        //    try
        //    {
        //        model.ResolveDependency(_scope);
        //        model.CreatePost();

        //        return Ok();
        //    }
        //    catch (Exception ex)
        //    {
        //        _logger.LogError(ex, "Couldn't delete Post");
        //        return BadRequest();
        //    }
        //}

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

        //[HttpPut]
        //public IActionResult Put(ViewPostRequestHandler model)
        //{
        //    try
        //    {
        //        model.ResolveDependency(_scope);
        //        model.UpdatePost();

        //        return Ok();
        //    }
        //    catch (Exception ex)
        //    {
        //        _logger.LogError(ex, "Couldn't delete Post");
        //        return BadRequest();
        //    }
        //} 

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
