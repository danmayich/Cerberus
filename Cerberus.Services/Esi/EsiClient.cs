using Cerberus.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Runtime.InteropServices;
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

        private async Task<T[]> GetPagedAsyncOrders<T>(string url, string accessToken)
        {
            var results = new List<T>();
            int page = 1;

            while (true)
            {
                var req = new HttpRequestMessage(HttpMethod.Get, $"{url}");
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


        private async Task<T[]> PostRequest<T>(string url, string accessToken, object? body = null)
        {
            var results = new List<T>();

            var req = new HttpRequestMessage(HttpMethod.Post, url);
            req.Headers.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

            if (body != null)
            {
                var jsonBody = JsonSerializer.Serialize(body);
                req.Content = new StringContent(jsonBody, Encoding.UTF8, "application/json");
            }

            var res = await _http.SendAsync(req);
            res.EnsureSuccessStatusCode();

            var json = await res.Content.ReadAsStringAsync();
            var pageData = JsonSerializer.Deserialize<T[]>(json)!;

            results.AddRange(pageData);
            return results.ToArray();
        }

        public Task<MarketOrderDto[]> GetMarketOrderDtosAsync(string accessToken, string itemTypeId)
        {
            return GetPagedAsyncOrders<MarketOrderDto>($"https://esi.evetech.net/latest/markets/10000002/orders?order_type=buy&type_id={itemTypeId}&page=1", accessToken);
        }

        public Task<EsiAsset[]> GetCharacterAssetsAsync(long characterId, string accessToken) => GetPagedAsync<EsiAsset>($"https://esi.evetech.net/latest/characters/{characterId}/assets/", accessToken);

        public Task<WalletJournalEntry[]> GetCharacterWalletJournal(long characterId, string accessToken) => GetPagedAsync<WalletJournalEntry>($"https://esi.evetech.net/characters/{characterId}/wallet/journal", accessToken);

        /// <summary>
        /// This ESI end point seems broken all the responses just say "Name None"
        /// </summary>
        /// <param name="characterId"></param>
        /// <param name="accessToken"></param>
        /// <param name="itemIds"></param>
        /// <returns></returns>
        public Task<EsiAssetNames[]> GetCharacterAssetNamesAsync(long characterId, string accessToken, long[] itemIds) => PostRequest<EsiAssetNames>($"https://esi.evetech.net/latest/characters/{characterId}/assets/names", accessToken, itemIds);

        public Task<EsiMarketOrder[]> GetCharacterOrdersAsync(long characterId, string accessToken) => GetPagedAsync<EsiMarketOrder>($"https://esi.evetech.net/latest/characters/{characterId}/orders/", accessToken);


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
