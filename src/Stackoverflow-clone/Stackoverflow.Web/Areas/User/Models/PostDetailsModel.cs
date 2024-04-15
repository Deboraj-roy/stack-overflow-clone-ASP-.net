using Autofac;
using Newtonsoft.Json;
using Stackoverflow.Application.Features.Services;
using Stackoverflow.Domain.Entities;
using Stackoverflow.Infrastructure;
using System.Web;

namespace Stackoverflow.Web.Areas.User.Models
{
    public class PostDetailsModel
    {
        private ILifetimeScope _scope;
        private IPostManagementService _postManagementService;
        private readonly HttpClient _httpClient;

        public PostDetailsModel()
        {
        }

        public PostDetailsModel(IPostManagementService postManagementService)
        {
            _postManagementService = postManagementService;
            _httpClient = new HttpClient();
            _httpClient.BaseAddress = new Uri("https://localhost:7278/v3/");
        }

        public void Resolve(ILifetimeScope scope)
        {
            _scope = scope;
            _postManagementService = _scope.Resolve<IPostManagementService>();
        }
           
        public async Task<Post> GetPostsDetailsAsync(Guid postId)
        {
            var response = await _httpClient.GetAsync($"Post/{postId}");
            response.EnsureSuccessStatusCode(); // Throw exception if not success

            var content = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<Post>(content);
        }
    }
}
