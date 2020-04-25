using IdentityServer4;
using IdentityServer4.Models;
using System.Collections.Generic;

namespace WindowsAuthSpike
{
    public class IdentityServerConfig
    {
        public static IEnumerable<IdentityResource> Ids => new IdentityResource[]
            {
                new IdentityResources.OpenId(),
                new IdentityResources.Profile()
            };

        public static IEnumerable<ApiResource> Apis =>
            new ApiResource[]
            {
                new ApiResource("poManagerApi")
            };

        public static IEnumerable<Client> Clients =>
            new List<Client> {
                new Client
                {
                    AccessTokenLifetime = 60,
                    AllowedCorsOrigins =
                    {
                        "http://localhost:3000"
                    },
                    AllowedGrantTypes = GrantTypes.Code,
                    AllowedScopes = {  IdentityServerConstants.StandardScopes.OpenId,
                        IdentityServerConstants.StandardScopes.Profile,
                        "poManagerApi" },
                    ClientId = "poManagerFrontEnd",
                    RequireClientSecret = false,
                    RequireConsent = false,
                    PostLogoutRedirectUris = { "http://localhost:3000/oidc_logout_callback" },
                    RedirectUris = { "http://localhost:3000/oidc_callback", "http://localhost:3000/oidc_silent_callback" },
                    RequirePkce = true
                }
            };
    }
}
