using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using System.Net.Http;
using System.Net.Http.Headers;

namespace KD25_BitirmeProjesi.UI.MVC_Core.Helpers
{
    public static class CustomHttpClientHelper
    {
        public static async Task<T> GetAsync<T>(IHttpClientFactory httpClientFactory, string token, string url)
        {
            var client = httpClientFactory.CreateClient();

            var request = new HttpRequestMessage(HttpMethod.Get, url);
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var response = await client.SendAsync(request);

            if (!response.IsSuccessStatusCode)
            {
                throw new Exception("Veriler alınırken bir hata oluştu.");
            }

            var json = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<T>(json);
        }
    }
}
