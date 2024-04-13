using Autofac;
using Stackoverflow.Application.Features.Services;
using Stackoverflow.Infrastructure;
using System.Web;

namespace Stackoverflow.Web.Areas.User.Models
{
    public class PostListModel
    {

        private ILifetimeScope _scope;
        private IPostManagementService _postManagementService;
        public PostSearch searchTitle { get; set; }
          
        public PostListModel()
        {
        }

        public PostListModel(IPostManagementService postManagementService)
        {
            _postManagementService = postManagementService;
        }

        public void Resolve(ILifetimeScope scope)
        {
            _scope = scope;
            _postManagementService = _scope.Resolve<IPostManagementService>();
        }

        public async Task<object> GetPagedCoursesAsync(DataTablesAjaxRequestUtility dataTablesUtility)
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

        internal async Task DeleteCourseAsync(Guid id)
        {
            await _postManagementService.DeletePostAsync(id);
        }

    }
}
