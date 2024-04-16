using Autofac;
using Newtonsoft.Json;
using Stackoverflow.Application.Features.Services;
using Stackoverflow.Domain.Entities;
using Stackoverflow.Infrastructure;
using System.Net;
using System.Net.Http.Headers;
using System.Text;
using System.Web;

namespace Stackoverflow.Web.Areas.User.Models
{
    public class PostSearchModel
    {
        private ILifetimeScope _scope;
        private IPostManagementService _postManagementService;
        public string Title { get; set; }
        private readonly HttpClient _httpClient;

        public PostSearchModel()
        {
        }

        public PostSearchModel(IPostManagementService postManagementService)
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
                Title,
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


        public async Task<Post[]> GetPostsSearchAPIAsync(string token)
        {
            try
            {
                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

                //var searchPayload = new
                //{
                //    start = 0,
                //    length = 0,
                //    order = new[]
                //    {
                //    new
                //    {
                //        column = 0,
                //        dir = "string"
                //    }
                //},
                //    search = new
                //    {
                //        regex = true,
                //        value = "string"
                //    },
                //    searchItem = new
                //    {
                //        title = Title
                //    }
                //};

                //var requestContent = new StringContent(JsonConvert.SerializeObject(searchPayload), Encoding.UTF8, "application/json");

                //var response = await _httpClient.PostAsync("Post/view", requestContent);
                //response.EnsureSuccessStatusCode(); // Throw exception if not success

                //var content = await response.Content.ReadAsStringAsync();

                //return JsonConvert.DeserializeObject<Post[]>(content);


                var searchPayload = new
                {
                    SearchItem = new
                    {
                        Title,
                        
                    }
                };

                var requestContent = new StringContent(JsonConvert.SerializeObject(searchPayload), Encoding.UTF8, "application/json");

                var response = await _httpClient.PostAsync("Post/view", requestContent);
                response.EnsureSuccessStatusCode(); // Throw exception if not success

                var content = await response.Content.ReadAsStringAsync();

                return JsonConvert.DeserializeObject<Post[]>(content);
            }
            catch (HttpRequestException ex) when (ex.StatusCode == HttpStatusCode.Unauthorized)
            {
                // Handle 401 Unauthorized exception
                // Redirect to login page or display a message asking the user to login again
                throw new UnauthorizedAccessException("Unauthorized access"); // Throw an exception to be handled further up the call stack
            }
            catch (Exception ex)
            {
                // Handle other exceptions
                throw;
            }
        }
    }

}
