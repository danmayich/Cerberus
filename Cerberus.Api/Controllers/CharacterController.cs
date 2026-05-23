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
        [HttpGet()]
        public async Task<CharacterDto> GetCharacter()
        {
            var (charId, accessToken, refreshToken) = GetTokens();

            var character = await characterApplication.LoadCharacter(charId, accessToken);

            return character;
        }

        [Authorize]
        [HttpPost("track-position")]
        public IActionResult TrackPosition([FromBody] EsiWalletTransaction transaction)
        {
            if (transaction is null)
            {
                return BadRequest("Transaction payload is required.");
            }

            var (charId, _, _) = GetTokens();
            characterApplication.TrackPosition(charId, transaction);

            return Ok(new
            {
                tracked = true,
                transactionId = transaction.TransactionId,
                itemName = transaction.ItemName,
                date = transaction.Date
            });
        }

        [Authorize]
        [HttpDelete("track-position/{transactionId:long}")]
        public IActionResult UntrackPosition([FromRoute] long transactionId)
        {
            var (charId, _, _) = GetTokens();
            characterApplication.UntrackPosition(charId, transactionId);

            return Ok(new
            {
                tracked = false,
                transactionId
            });
        }

        private (long charId, string accessToken, string refreshToken) GetTokens()
        {
            var charIdClaim = User.FindFirstValue("character_id");
            if (string.IsNullOrWhiteSpace(charIdClaim) || !long.TryParse(charIdClaim, out var charId))
                throw new UnauthorizedAccessException("Missing or invalid character_id claim");

            var accessToken = User.FindFirst("access_token")?.Value; // depends on your setup
            var refreshToken = User.FindFirst("refresh_token")?.Value;

            if (string.IsNullOrEmpty(accessToken))
                throw new UnauthorizedAccessException("Missing access token");

            if (string.IsNullOrEmpty(refreshToken))
                throw new UnauthorizedAccessException("Missing refresh token");

            return (charId, accessToken, refreshToken);
        }
    }
}
