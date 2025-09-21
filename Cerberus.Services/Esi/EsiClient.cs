using Cerberus.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Cerberus.Services.Esi
{
    public class EsiClient
    {
        private readonly HttpClient _http;

        public EsiClient(HttpClient http)
        {
            _http = http;
        }

        private async Task<T[]> GetPagedAsync<T>(string url, string accessToken)
        {
            var results = new List<T>();
            int page = 1;

            while (true)
            {
                var req = new HttpRequestMessage(HttpMethod.Get, $"{url}?page={page}");
                req.Headers.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

                var res = await _http.SendAsync(req);
                if (!res.IsSuccessStatusCode)
                    throw new Exception($"ESI error {res.StatusCode}: {await res.Content.ReadAsStringAsync()}");

                var json = await res.Content.ReadAsStringAsync();
                var pageData = JsonSerializer.Deserialize<T[]>(json)!;
                if (pageData.Length == 0)
                    break;

                results.AddRange(pageData);

                if (!res.Headers.TryGetValues("X-Pages", out var values))
                    break;

                int totalPages = int.Parse(values.First());
                if (page >= totalPages)
                    break;

                page++;
            }

            return results.ToArray();
        }

        public Task<EsiAsset[]> GetCharacterAssetsAsync(long characterId, string accessToken) =>
            GetPagedAsync<EsiAsset>($"https://esi.evetech.net/latest/characters/{characterId}/assets/", accessToken);

        public Task<EsiMarketOrder[]> GetCharacterOrdersAsync(long characterId, string accessToken) =>
            GetPagedAsync<EsiMarketOrder>($"https://esi.evetech.net/latest/characters/{characterId}/orders/", accessToken);


        public async Task<List<EsiWalletTransaction>> GetWalletTransactionsAsync(long characterId, string accessToken)
        {
            using var client = new HttpClient();
            client.DefaultRequestHeaders.Authorization =
                new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", accessToken);

            var url = $"https://esi.evetech.net/latest/characters/{characterId}/wallet/transactions/";
            var response = await client.GetAsync(url);

            response.EnsureSuccessStatusCode();

            var json = await response.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<List<EsiWalletTransaction>>(json);
        }
    }
}
