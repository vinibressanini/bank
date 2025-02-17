using desafioAPI.Exceptions;
using desafioAPI.Models;
using System.Text.Json;

namespace desafioAPI.Services
{
    public class AuthorizationService
    {

        private HttpClient _httpClient;

        public AuthorizationService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<bool> Authorize(Transaction transaction)
        {

            var response= await _httpClient.GetAsync("https://util.devi.tools/api/v2/authorize");

            Authorization authorization = await response.Content.ReadFromJsonAsync<Authorization>();

            if (authorization.isAuthorized()) return true;

            throw new AuthorizationException("The transaction wasn't allowed to be finished", transaction);

        }


    }
}
