using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Cerberus.Api.Controllers.Authentication
{
    [ApiController]
    [Route("[controller]")]
    public class AuthenticationController : ControllerBase
    {
        /// <summary>
        /// Log into ESI using CCPs OIDC
        /// </summary>
        /// <param name="returnUrl">The redirect url to navigate to once login completes.</param>
        /// <returns></returns>
        [HttpGet("login")]
        public IActionResult Login([FromQuery] string returnUrl = "/character")
        {
            return Challenge(new AuthenticationProperties
            {
                RedirectUri = returnUrl
            }, "EveSSO");
        }


        /// <summary>
        /// Log out.
        /// </summary>
        /// <returns></returns>
        [HttpGet("logout")]
        public async Task<IActionResult> Logout([FromQuery] string returnUrl = "http://localhost:8080")
        {
            // Local sign-out (cookie auth).
            // 'oidc' is not registered in this app; OAuth challenge scheme is 'EveSSO'.
            await HttpContext.SignOutAsync("Cookies");
            return Redirect(returnUrl);
        }

        /// <summary>
        /// Log out.
        /// </summary>
        /// <returns></returns>
        [Authorize]
        [HttpGet("verify")]
        public async Task<IActionResult> Verify()
        {
            // After successful login you can read stored tokens
            var accessToken = await HttpContext.GetTokenAsync("access_token");
            var refreshToken = await HttpContext.GetTokenAsync("refresh_token");
            var expiresAt = await HttpContext.GetTokenAsync("expires_at"); // string timestamp

            // Persist refreshToken server-side (DB) keyed to your user/character ID
            // Do NOT permanently rely only on the cookie for long-term storage.

            return Content($"access_token: {(accessToken != null ? "[present]" : "[missing]")}\n" +
                           $"refresh_token: {(refreshToken != null ? "[present]" : "[missing]")}\n" +
                           $"expires_at: {expiresAt}");
        }
    }
}
