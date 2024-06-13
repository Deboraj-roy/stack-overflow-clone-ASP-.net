using Autofac;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Stackoverflow.Application.Features.Services;
using Stackoverflow.Domain.Entities;
using Stackoverflow.Infrastructure;
using Stackoverflow.Web.Models;
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
        private string _baseAddress = System.Environment.GetEnvironmentVariable("API_URL") ?? WebConstants.ApiUrl;

        private readonly ILogger<PostListModel> _logger;
        public PostListModel()
        {
        }

        public PostListModel(IPostManagementService postManagementService, ILogger<PostListModel> logger)
        {
            _postManagementService = postManagementService;
            _httpClient = new HttpClient();
            _httpClient.BaseAddress = new Uri(_baseAddress);
            _logger = logger;
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

                var r = _httpClient.DefaultRequestHeaders.Authorization.ToString();
                _logger.LogInformation($"Authorization: {r}");

                var c = _httpClient.BaseAddress.ToString();
                _logger.LogInformation($"BaseAddress: {c}");

                var response = await _httpClient.GetAsync("Post");
                response.EnsureSuccessStatusCode(); // Throw exception if not success

                var content = await response.Content.ReadAsStringAsync();

                _logger.LogInformation(content);

                return JsonConvert.DeserializeObject<Post[]>(content);
            }
            catch (HttpRequestException ex) when (ex.StatusCode == HttpStatusCode.Unauthorized)
            {

                var r = _httpClient.DefaultRequestHeaders.Authorization.ToString();
                _logger.LogInformation($"Authorization: {r}");
                // Handle 401 Unauthorized exception
                // Redirect to login page or display a message asking the user to login again
                // throw; // Rethrow the exception if you want to handle it further up the call stack
                _logger.LogError(ex.Message);
                _logger.LogError(ex.StackTrace);
                _logger.LogError(ex.StatusCode.ToString());
                return null; // Return null if an exception occurs
            }
            catch (Exception ex)
            {
                // Handle other exceptions
                // throw;
                var c = _httpClient.BaseAddress.ToString();
                _logger.LogInformation($"BaseAddress: {c}");
                _logger.LogError(ex.Message);
                return null; // Return null if an exception occurs
            }
        }

    }

}
