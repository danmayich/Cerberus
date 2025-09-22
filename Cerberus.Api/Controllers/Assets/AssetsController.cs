using Cerberus.Application.Assets;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

[ApiController]
[Route("[controller]")]
public class AssetsController(AssetRetrievalApplication assetRetrievalApplication) : ControllerBase
{
    [HttpGet("assets")]
    public async Task<IActionResult> GetAssets()
    {
        var (charId, accessToken, refreshToken) = GetTokens();
        var assets = await assetRetrievalApplication.GetAssets(charId, accessToken);
        return Ok(assets);
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
