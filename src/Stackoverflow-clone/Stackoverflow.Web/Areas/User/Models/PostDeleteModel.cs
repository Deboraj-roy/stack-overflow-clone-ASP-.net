using Autofac;
using Stackoverflow.Application.Features.Services;
using Stackoverflow.Web.Models;

namespace Stackoverflow.Web.Areas.User.Models
{
    public class PostDeleteModel
    {
        private ILifetimeScope _scope;
        private IPostManagementService _postManagementService;
        public PostSearch searchTitle { get; set; }
        private readonly HttpClient _httpClient;
        private string _baseAddress = System.Environment.GetEnvironmentVariable("API_URL") ?? WebConstants.ApiUrl;

        public PostDeleteModel()
        {
        }

        public PostDeleteModel(IPostManagementService postManagementService)
        {
            _postManagementService = postManagementService;
            _httpClient = new HttpClient(); 
            _httpClient.BaseAddress = new Uri(_baseAddress);
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
            var response = await _httpClient.DeleteAsync($"Post/{id}");
            response.EnsureSuccessStatusCode(); // Throw exception if not successful
        }
         
    }

}
