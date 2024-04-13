using Autofac;
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

        [HttpPost]
        //[HttpPost, Authorize(Policy = "PostViewRequirementPolicy")]
        public async Task<object> Post([FromBody] ViewPostRequestHandler handler)
        {
            handler.ResolveDependency(_scope);
            var data = await handler.GetPagedPosts();
            _logger.LogInformation("Posts found");
            return data;
        }


        [HttpGet]
        //[HttpGet, Authorize(Policy = "PostViewRequirementPolicy")]
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
        public IActionResult Delete(Guid id)
        {
            try
            {
                var model = _scope.Resolve<ViewPostRequestHandler>();
                model.DeletePost(id);
                _logger.LogInformation("Post deleted");
                return Ok();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Couldn't delete Post");
                return BadRequest();
            }
        }
    }
}
