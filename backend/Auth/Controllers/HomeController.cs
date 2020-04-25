using IdentityServer4.Services;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace WindowsAuthSpike.Controllers.IdentityServer
{
    public class HomeController : ControllerBase
    {
        private IIdentityServerInteractionService _identityServerInteractionService;

        public HomeController(IIdentityServerInteractionService identityServerInteractionService)
        {
            _identityServerInteractionService = identityServerInteractionService;
        }

        [HttpGet]
        public async Task<IActionResult> Error(string errorId)
        {
            var errorMessage = await _identityServerInteractionService.GetErrorContextAsync(errorId);
            return Ok(errorMessage);
        }
    }
}