using Autofac;
using Newtonsoft.Json;
using Stackoverflow.Domain.Entities;
using Stackoverflow.Web.Models;

namespace Stackoverflow.Web.Areas.User.Models
{
    public class PostSearchModel
    {
        private ILifetimeScope _scope;
        public string Title { get; set; }
        private readonly HttpClient _httpClient;
        private string _baseAddress = System.Environment.GetEnvironmentVariable("API_URL") ?? WebConstants.ApiUrl;

        public PostSearchModel()
        {
            _httpClient = new HttpClient(); 
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
         
    }

}
