using IdentityModel;
using IdentityServer;
using IdentityServer4;
using IdentityServer4.Events;
using IdentityServer4.Services;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Server.IISIntegration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Security.Principal;
using System.Threading.Tasks;

namespace WindowsAuthSpike.Controllers.IdentityServer
{
    [SecurityHeaders]
    [AllowAnonymous]
    [Route("[controller]")]
    public class ExternalController : ControllerBase
    {
        private readonly IEventService _events;
        private readonly IIdentityServerInteractionService _interaction;

        public ExternalController (IEventService events, IIdentityServerInteractionService interaction)
        {
            _events = events;
            _interaction = interaction;
        }

        [HttpGet("Challenge")]
        public async Task<IActionResult> Challenge(string returnUrl)
        {
            if (string.IsNullOrEmpty(returnUrl)) returnUrl = "~/";

            if (Url.IsLocalUrl(returnUrl) == false && _interaction.IsValidReturnUrl(returnUrl) == false)
            {
                throw new Exception("invalid return URL");
            }

            return await ProcessWindowsLoginAsync(returnUrl);
        }

        [HttpGet("Callback")]
        public async Task<IActionResult> Callback()
        {
            var result = await HttpContext.AuthenticateAsync(IdentityServerConstants.ExternalCookieAuthenticationScheme);
            if (result?.Succeeded != true)
            {
                throw new Exception("External authentication error");
            }

            // Retrieve user claims from OUR database here and generate any additional local claims desired
            var user = new
            {
                Sub = result.Principal.FindFirst("sub").Value,
                Name = result.Principal.FindFirst("name").Value
            };

            var isuser = new IdentityServerUser(user.Sub)
            {
                DisplayName = user.Name,
                IdentityProvider = IISDefaults.AuthenticationScheme
            };

            var additionalLocalClaims = new List<Claim>();

            var localSignInProps = new AuthenticationProperties();
            ProcessLoginCallbackForOidc(result, additionalLocalClaims, localSignInProps);

            await HttpContext.SignInAsync(isuser, localSignInProps);

            await HttpContext.SignOutAsync(IdentityServerConstants.ExternalCookieAuthenticationScheme);

            var returnUrl = result.Properties.Items["returnUrl"] ?? "~/";

            // check if external login is in the context of an OIDC request
            var context = await _interaction.GetAuthorizationContextAsync(returnUrl);
            await _events.RaiseAsync(new UserLoginSuccessEvent(IdentityServerConstants.ExternalCookieAuthenticationScheme, user.Name, user.Sub, user.Name, true, context?.ClientId));

            return Redirect(returnUrl);
        }

        private void ProcessLoginCallbackForOidc(AuthenticateResult externalResult, List<Claim> localClaims, AuthenticationProperties localSignInProps)
        {
            // if the external system sent a session id claim, copy it over
            // so we can use it for single sign-out
            var sid = externalResult.Principal.Claims.FirstOrDefault(x => x.Type == JwtClaimTypes.SessionId);
            if (sid != null)
            {
                localClaims.Add(new Claim(JwtClaimTypes.SessionId, sid.Value));
            }

            // if the external provider issued an id_token, we'll keep it for signout
            var id_token = externalResult.Properties.GetTokenValue("id_token");
            if (id_token != null)
            {
                localSignInProps.StoreTokens(new[] { new AuthenticationToken { Name = "id_token", Value = id_token } });
            }
        }

        private async Task<IActionResult> ProcessWindowsLoginAsync(string returnUrl)
        {
            // see if windows auth has already been requested and succeeded
            var result = await HttpContext.AuthenticateAsync(IISDefaults.AuthenticationScheme);
            if (result?.Principal is WindowsPrincipal wp)
            {
                var props = new AuthenticationProperties()
                {
                    RedirectUri = Url.Action("Callback"),
                    Items =
                    {
                        { "returnUrl", returnUrl },
                        { "scheme", IISDefaults.AuthenticationScheme }
                    }
                };

                var id = new ClaimsIdentity(IISDefaults.AuthenticationScheme);
                id.AddClaim(new Claim(JwtClaimTypes.Subject, wp.FindFirst(ClaimTypes.PrimarySid).Value));
                id.AddClaim(new Claim(JwtClaimTypes.Name, wp.Identity.Name));

                await HttpContext.SignInAsync(
                    IdentityServerConstants.ExternalCookieAuthenticationScheme,
                    new ClaimsPrincipal(id),
                    props);
                return Redirect(props.RedirectUri);
            }
            else
            {
                // trigger windows auth
                // since windows auth don't support the redirect uri,
                // this URL is re-triggered when we call challenge
                return base.Challenge(IISDefaults.AuthenticationScheme);
            }
        }
    }
}