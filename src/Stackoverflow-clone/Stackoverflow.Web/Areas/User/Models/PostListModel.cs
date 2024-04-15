using Autofac;
using Newtonsoft.Json;
using Stackoverflow.Application.Features.Services;
using Stackoverflow.Domain.Entities;
using Stackoverflow.Infrastructure;
using System.Web;

namespace Stackoverflow.Web.Areas.User.Models
{
    public class PostListModel
    { 
        private ILifetimeScope _scope;
        private IPostManagementService _postManagementService;
        public PostSearch searchTitle { get; set; }
        private readonly HttpClient _httpClient;

        public PostListModel()
        {
        }

        public PostListModel(IPostManagementService postManagementService)
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

        public async Task<object> GetPagedPostsAsync(DataTablesAjaxRequestUtility dataTablesUtility)
        {
            var data = await _postManagementService.GetPagedPostsAsync(
                dataTablesUtility.PageIndex,
                dataTablesUtility.PageSize,
                searchTitle.Title,
                dataTablesUtility.GetSortText(new string[] { "Title" })
                );

            return new
            {
                recordsTotal = data.total,
                recordsFiltered = data.totalDisplay,
                data = (from record in data.records
                        select new string[]
                        {
                                HttpUtility.HtmlEncode(record.Title),
                                record.Id.ToString()
                        }
                    ).ToArray()
            };
        }
         

        public async Task<Post[]> GetPostsAsync()
        {
            var response = await _httpClient.GetAsync("Post");
            response.EnsureSuccessStatusCode(); // Throw exception if not success

            var content = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<Post[]>(content);
        }

    }
}
