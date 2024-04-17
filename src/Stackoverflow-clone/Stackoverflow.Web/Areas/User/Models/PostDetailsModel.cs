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
        private readonly HttpClient _httpClient;
        private string _baseAddress;

        public PostDetailsModel()
        {
            _httpClient = new HttpClient();
            _baseAddress = DetermineBaseAddress();
            _httpClient.BaseAddress = new Uri(_baseAddress);
        }

        public void Resolve(ILifetimeScope scope)
        {
            _scope = scope;
        }

        public async Task<Post> GetPostsDetailsAsync(Guid postId)
        {
            var response = await _httpClient.GetAsync($"Post/{postId}");
            response.EnsureSuccessStatusCode(); // Throw exception if not success

            var content = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<Post>(content);
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
