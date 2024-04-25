using Autofac;
using Newtonsoft.Json;
using Stackoverflow.Domain.Entities;
using Stackoverflow.Web.Models;

namespace Stackoverflow.Web.Areas.User.Models
{
    public class PostDetailsModel
    {
        private ILifetimeScope _scope;
        private readonly HttpClient _httpClient;
        private string _baseAddress = WebConstants.ApiUrl;

        public PostDetailsModel()
        {
            _httpClient = new HttpClient(); 
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
         
    }

}
