using Autofac;
using Microsoft.Extensions.Hosting;
using Newtonsoft.Json;
using Stackoverflow.Application.Features.Services;
using Stackoverflow.Domain.Entities;
using Stackoverflow.Web.Models;

namespace Stackoverflow.Web.Areas.User.Models
{
    public class PostDetailsModel
    {
        private ILifetimeScope _scope;
        private readonly HttpClient _httpClient;
        private ICommentManagementService _commentManagementService;
        public Post? Post { get; set; } = new Post();
        public CommentCreateModel? Comment { get; set; } = new CommentCreateModel();
        public List<Comment>? Comments { get; set; }
         
        private string _baseAddress = System.Environment.GetEnvironmentVariable("API_URL") ?? "http://localhost:5293/v3/";
        public Guid? postId { get; set; }

        public PostDetailsModel()
        {
            
        }

        public PostDetailsModel(ICommentManagementService commentManagementService)
        {
            _httpClient = new HttpClient(); 
            _httpClient.BaseAddress = new Uri(_baseAddress);
            _commentManagementService = commentManagementService;
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

        public async Task<List<Comment>> GetCommentsAsync(Guid postId)
        {
            //return await _commentManagementService.GetCommentsAsync(postId);
            //return NotImplementedException();
            var comments = await _commentManagementService.GetCommentListAsync();

            //var filteredComments = comments.Where(com => com.PostId.Equals(postId)); // Filter posts based on the search query

            //return (List<Comment>)filteredComments;

            return comments
                  .Where(com => com.PostId.Equals(postId))
                  .ToList();

            //return await _commentManagementService.GetCommentsAsync();
        }

        private Comment NotImplementedException()
        {
            throw new NotImplementedException();
        }
    }

}
