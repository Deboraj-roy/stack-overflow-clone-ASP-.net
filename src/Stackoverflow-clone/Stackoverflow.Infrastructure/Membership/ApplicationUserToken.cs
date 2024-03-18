using System;

using Microsoft.AspNetCore.Identity;

namespace Stackoverflow.Infrastructure.Membership
{
    public class ApplicationUserToken
        : IdentityUserToken<Guid>
    {

    }
}
