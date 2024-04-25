using Autofac;
using AutoMapper;
using Stackoverflow.Application.Features.Services;
using Stackoverflow.Domain.Entities;
using Stackoverflow.Infrastructure;

namespace Stackoverflow.API.RequestHandlers
{
    public class ViewPostRequestHandler : DataTables
    {
        private IPostManagementService? _postService;
        private IMapper _mapper;
        public string Title { get; set; }
        public string Description { get; set; } = string.Empty;        public PostSearch SearchItem { get; set; }

        public ViewPostRequestHandler()
        {

        }

        public ViewPostRequestHandler(IPostManagementService postService, IMapper mapper)
        {
            _postService = postService;
            _mapper = mapper;
        }

        public void ResolveDependency(ILifetimeScope scope)
        {
            _postService = scope.Resolve<IPostManagementService>();
            _mapper = scope.Resolve<IMapper>();
        }

        internal async Task<IList<Post>>? GetPostsAsync()
        {
            return await _postService?.GetPostAsync();
        }

        internal async Task<bool> DeletePost(Guid id)
        {
            var post = await GetPostAsync(id);
            if (post != null)
            {
                await _postService?.DeletePostAPIAsync(id);
                return true;
            }
            else
            {
                return false;
            }
        }

        //internal Post GetPost(string name)
        //{
        //    return _postService.GetPost(name);
        //}

        internal async Task<Post>? GetPostAsync(Guid id)
        {
            return await _postService?.GetPostAsync(id);
        }

        internal async Task<object?> GetPagedPosts()
        {

            var data = await _postService?.GetPagedPostsAsync(
                PageIndex,
                PageSize,
                SearchItem.Title,
                FormatSortExpression("Title", "Description"));

            return new
            {
                recordsTotal = data.total,
                recordsFiltered = data.totalDisplay,
                data = (from record in data.records
                        select new string[]
                        {
                                record.Title,
                                record.Id.ToString()
                        }
                    ).ToArray()
            };
        }
    }
}
