using Autofac;
using Newtonsoft.Json;
using Stackoverflow.Application.Features.Services;
using Stackoverflow.Domain.Entities;
using Stackoverflow.Infrastructure;
using System.Web;

namespace Stackoverflow.Web.Areas.User.Models
{
    public class PostDeleteModel
    { 
        private ILifetimeScope _scope;
        private IPostManagementService _postManagementService;
        public PostSearch searchTitle { get; set; }
        private readonly HttpClient _httpClient;

        public PostDeleteModel()
        {
        }

        public PostDeleteModel(IPostManagementService postManagementService)
        {
            _postManagementService = postManagementService;
            _httpClient = new HttpClient();
            _httpClient.BaseAddress = new Uri("https://localhost:5759/v3/");
        }

        public void Resolve(ILifetimeScope scope)
        {
            _scope = scope;
            _postManagementService = _scope.Resolve<IPostManagementService>();
        }

        internal async Task DeletePostAsync(Guid id)
        {
            await _postManagementService.DeletePostAsync(id);
        }

        internal async Task DeletePostAsyncAPI(Guid id)
        {
            //var response = await _httpClient.GetAsync($"Post/{postId}");
            //response.EnsureSuccessStatusCode(); // Throw exception if not success

            //var content = await response.Content.ReadAsStringAsync();
            //return JsonConvert.DeserializeObject<Post>(content);

            var response = await _httpClient.DeleteAsync($"https://localhost:7278/v3/Post/{id}");
            response.EnsureSuccessStatusCode(); // Throw exception if not successful

        }
    }
}
