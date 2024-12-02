using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Net.WebRequestMethods;

namespace RecipeBook.Services
{
    public class RecipeService
    {
        private readonly HttpClient _httpClient;

        private readonly string baseUrl = "https://19f1-2600-6c44-78f0-a450-f592-dabf-1b77-6aa0.ngrok-free.app";

        public RecipeService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<string> GetAllRecipesAsync()
        {
            var request = new HttpRequestMessage
            {
                Method = HttpMethod.Get,
                RequestUri = new Uri(baseUrl + "/api/Recipe"),
            };

            using (var response = await _httpClient.SendAsync(request))
            {
                response.EnsureSuccessStatusCode();
                var body = await response.Content.ReadAsStringAsync();
                return body;
            }
        }

        public async Task DeleteRecipe(string id)
        {
            var request = new HttpRequestMessage
            {
                Method = HttpMethod.Delete,
                RequestUri = new Uri($"{baseUrl}/api/Recipe/{id}"),
            };

            using (var response = await _httpClient.SendAsync(request))
            {
                response.EnsureSuccessStatusCode();
            }
        }

        public async Task UpdateRecipe(string body, string id)
        {
            var request = new HttpRequestMessage
            {
                Method = HttpMethod.Put,
                RequestUri = new Uri(baseUrl + "/api/Recipe/" + id),
                Content = new StringContent(body, Encoding.UTF8, "application/json"),
            };

            using (var response = await _httpClient.SendAsync(request))
            {
                response.EnsureSuccessStatusCode();
            }
        }

        public async Task AddRecipe(string body)
        {
            var request = new HttpRequestMessage
            {
                Method = HttpMethod.Post,
                RequestUri = new Uri(baseUrl + "/api/Recipe"),
                Content = new StringContent(body, Encoding.UTF8, "application/json"),
            };

            using (var response = await _httpClient.SendAsync(request))
            {
                response.EnsureSuccessStatusCode();
            }
        }
    }
}
