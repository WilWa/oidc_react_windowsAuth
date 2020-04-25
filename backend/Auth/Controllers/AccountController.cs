using IdentityServer;
using IdentityServer4.Models;
using IdentityServer4.Services;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace WindowsAuthSpike.Controllers.IdentityServer
{
    [SecurityHeaders]
    [AllowAnonymous]
    public class AccountController : ControllerBase
    {
        private readonly IIdentityServerInteractionService _interaction;

        public AccountController(IIdentityServerInteractionService interaction)
        {
            _interaction = interaction;
        }

        [HttpGet]
        public IActionResult Login(string returnUrl)
        {
            return RedirectToAction("Challenge", "External", new { returnUrl });
        }

        [HttpGet]
        public async Task<IActionResult> Logout(string logoutId)
        {

            await HttpContext.SignOutAsync();
            LogoutRequest logoutRequest = await _interaction.GetLogoutContextAsync(logoutId);
            return Redirect(logoutRequest.PostLogoutRedirectUri);
        }
    }
}