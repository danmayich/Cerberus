using Cerberus.Application;
using Cerberus.Dto;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Cerberus.Api.Controllers.Authentication
{
    [ApiController]
    [Route("[controller]")]
    public class CharacterController(CharacterApplication characterApplication) : ControllerBase
    {
        /// <summary>
        /// Log out.
        /// </summary>
        /// <returns></returns>
        [Authorize]
        [HttpGet("character")]
        public async Task<CharacterDto> GetCharacter()
        {
            var (charId, accessToken, refreshToken) = GetTokens();

            var character = await characterApplication.LoadCharacter(charId, accessToken);

            return character;
        }

        private (long charId, string accessToken, string refreshToken) GetTokens()
        {
            var charId = long.Parse(User.FindFirstValue("character_id")!);
            var accessToken = User.FindFirst("access_token")?.Value; // depends on your setup
            var refreshToken = User.FindFirst("refresh_token")!.Value;

            if (string.IsNullOrEmpty(accessToken))
                throw new UnauthorizedAccessException("Missing access token");

            return (charId, accessToken, refreshToken);
        }
    }
}
