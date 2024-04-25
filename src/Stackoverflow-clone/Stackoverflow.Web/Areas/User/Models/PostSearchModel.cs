using Autofac;
using Newtonsoft.Json;
using Stackoverflow.Domain.Entities;

namespace Stackoverflow.Web.Areas.User.Models
{
    public class PostSearchModel
    {
        private ILifetimeScope _scope;
        public string Title { get; set; }
        private readonly HttpClient _httpClient;
        private string _baseAddress;

        public PostSearchModel()
        {
            _httpClient = new HttpClient();
            _baseAddress = DetermineBaseAddress();
            _httpClient.BaseAddress = new Uri(_baseAddress);
        }

        public void Resolve(ILifetimeScope scope)
        {
            _scope = scope;
        }

        public async Task<Post[]> GetPostsSearchAPIAsync()
        {
            if (string.IsNullOrEmpty(Title))
            {
                // Handle the case where the Title is null or empty
                return new Post[0]; // Return an empty array or handle it as per your requirement
            }

            var response = await _httpClient.GetAsync($"Post/search?title={Title}");
            response.EnsureSuccessStatusCode(); // Throw exception if not success

            var content = await response.Content.ReadAsStringAsync();

            return JsonConvert.DeserializeObject<Post[]>(content);
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
