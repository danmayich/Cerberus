
using Cerberus.Application;
using Cerberus.Application.Assets;
using Cerberus.Services;
using Cerberus.Services.Assets;
using Cerberus.Services.Data;
using Cerberus.Services.Esi;
using Cerberus.Settings;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Text.Json;

namespace Cerberus.Api;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);
        builder.AddServiceDefaults();

        // Add services to the container.

        var settings = new CerberusSettings()
        {
            ESISettings = new ESISettings()
            {
                ClientId = "40f15c65c0004e2e87bc75bbeabb3301",
                ClientSecret = "eat_1ZchAA3ww6Vpvh0r8BVzegv7WYYd65AaY_SQU5E"
            }
        };

        builder.Services.AddHttpClient<EsiClient>();
        builder.Services.AddSingleton(settings);

        builder.Services.AddSingleton(new CharacterRepository($"C:\\test\\esi\\"));

        builder.Services.AddScoped<AssetRetrievalService>();
        builder.Services.AddScoped<AssetRetrievalApplication>();
        builder.Services.AddScoped<WalletApplication>();
        builder.Services.AddScoped<WalletRetrievalService>();

        builder.Services.AddScoped<CharacterApplication>();

        builder.Services.AddAuthentication(options =>
        {
            options.DefaultScheme = "Cookies";
            options.DefaultChallengeScheme = "EveSSO";
        })
        .AddCookie("Cookies")
        .AddOAuth("EveSSO", options =>
        {
            //options.Authority = "https://login.eveonline.com";
            options.ClientId = settings.ESISettings.ClientId;// builder.Configuration["EveOnline:ClientId"];
            options.ClientSecret = settings.ESISettings.ClientSecret;// builder.Configuration["EveOnline:ClientSecret"];
                                                                     //options.ResponseType = "code";

            options.AuthorizationEndpoint = "https://login.eveonline.com/v2/oauth/authorize";
            options.TokenEndpoint = "https://login.eveonline.com/v2/oauth/token";

            options.CallbackPath = "/signin-oidc";
        
            // Do NOT use the default OIDC config (forces "openid")
            //options.Configuration = new Microsoft.IdentityModel.Protocols.OpenIdConnect.OpenIdConnectConfiguration
            //{
            //    AuthorizationEndpoint = "https://login.eveonline.com/v2/oauth/authorize",
            //    TokenEndpoint = "https://login.eveonline.com/v2/oauth/token"
            //};
        
            // Now you control scopes completely
            options.Scope.Clear();
            options.Scope.Add("esi-wallet.read_character_wallet.v1");
            options.Scope.Add("esi-assets.read_assets.v1");
            options.Scope.Add("esi-markets.structure_markets.v1");
            options.Scope.Add("esi-markets.read_character_orders.v1");

            options.SaveTokens = true;

            options.Events.OnCreatingTicket = async context =>
            {
                var http = new HttpClient();
                http.DefaultRequestHeaders.Authorization =
                    new AuthenticationHeaderValue("Bearer", context.AccessToken);

                var res = await http.GetStringAsync("https://esi.evetech.net/verify/");
                var verify = JsonDocument.Parse(res).RootElement;

                var charId = verify.GetProperty("CharacterID").GetInt64().ToString();
                var charName = verify.GetProperty("CharacterName").GetString();

                context.Identity!.AddClaim(new Claim("character_id", charId));
                context.Identity.AddClaim(new Claim("character_name", charName ?? ""));

                if (!string.IsNullOrEmpty(context.RefreshToken))
                    context.Identity.AddClaim(new Claim("refresh_token", context.RefreshToken));

                if (!string.IsNullOrEmpty(context.AccessToken))
                {
                    context.Identity.AddClaim(new Claim("access_token", context.AccessToken));
                }
            };
        });

        builder.Services.AddControllers();
        // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();

        var app = builder.Build();

        app.MapDefaultEndpoints();

        // Configure the HTTP request pipeline.
        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        app.UseHttpsRedirection();

        app.UseAuthorization();


        app.MapControllers();

        app.Run();
    }
}
