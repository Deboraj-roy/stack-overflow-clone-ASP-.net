using Autofac;
using Newtonsoft.Json;
using Stackoverflow.Application.Features.Services;
using Stackoverflow.Domain.Entities;
using Stackoverflow.Infrastructure;
using System.Net;
using System.Net.Http.Headers;
using System.Web;

namespace Stackoverflow.Web.Areas.User.Models
{
    public class PostListModel
    {
        private ILifetimeScope _scope;
        private IPostManagementService _postManagementService;
        public PostSearch searchTitle { get; set; }
        private readonly HttpClient _httpClient;
        private string _baseAddress;

        public PostListModel()
        {
        }

        public PostListModel(IPostManagementService postManagementService)
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

        public async Task<Post[]> GetPostsAsync(string token)
        {
            try
            {
                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

                var response = await _httpClient.GetAsync("Post");
                response.EnsureSuccessStatusCode(); // Throw exception if not success

                var content = await response.Content.ReadAsStringAsync();

                return JsonConvert.DeserializeObject<Post[]>(content);
            }
            catch (HttpRequestException ex) when (ex.StatusCode == HttpStatusCode.Unauthorized)
            {
                // Handle 401 Unauthorized exception
                // Redirect to login page or display a message asking the user to login again
                throw; // Rethrow the exception if you want to handle it further up the call stack
            }
            catch (Exception ex)
            {
                // Handle other exceptions
                throw;
            }
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
