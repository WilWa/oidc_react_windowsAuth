using IdentityServer4.Models;
using IdentityServer4.Services;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Auth
{
    public class ProfileService : IProfileService
    {
        public Task GetProfileDataAsync(ProfileDataRequestContext context)
        {
            string name = context.Subject.FindFirst("name").Value;
            context.IssuedClaims.Add(new Claim("name", name));
            return Task.CompletedTask;
        }

        public Task IsActiveAsync(IsActiveContext context)
        {
            context.IsActive = true;
            return Task.CompletedTask;
        }
    }
}
