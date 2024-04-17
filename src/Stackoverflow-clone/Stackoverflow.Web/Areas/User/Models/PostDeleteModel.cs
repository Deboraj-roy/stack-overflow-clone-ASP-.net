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
        private string _baseAddress;

        public PostDeleteModel()
        {
        }

        public PostDeleteModel(IPostManagementService postManagementService)
        {
            _postManagementService = postManagementService;
            _httpClient = new HttpClient();
            _baseAddress = DetermineBaseAddress();
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

        private string DetermineBaseAddress()
        {
            var environment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");

            switch (environment)
            {
                case "Development":
                    return "http://localhost:5293/v3/";
                default:
                    return "https://localhost:7278/v3/";
                //default:
                //    return "http://localhost:26441/v3/"; // Update with your IIS application URL
            }
        }
    }

}
