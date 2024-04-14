using Microsoft.AspNetCore.Authorization;
using System.Threading.Tasks;

namespace Stackoverflow.Infrastructure.Requirements
{
    public class PostViewRequirementHandler :
          AuthorizationHandler<PostViewRequirement>
    {
        protected override Task HandleRequirementAsync(
               AuthorizationHandlerContext context,
               PostViewRequirement requirement)
        {
            if (context.User.HasClaim(x => x.Type == "ViewPost" && x.Value == "true"))
            {
                context.Succeed(requirement);
            }

            return Task.CompletedTask;
        }
    }
}
