using DocumentAPI.DTOs;
using DocumentAPI.Interfaces;

namespace DocumentAPI.Services
{
    public class ExternalAPIService: IExternalAPIService
    {
        private HttpClient _httpClient;
        public ExternalAPIService(IHttpClientFactory httpClientFactory) 
        { 
            _httpClient = httpClientFactory.CreateClient();
        }

        public async Task<Result> GetRandomUserAsync()
        {
            var result = await _httpClient.GetFromJsonAsync<RandomUserResponseDto>("https://randomuser.me/api/");
            if (result == null)
            {
                throw new Exception("Get no result form endpoint");
            }
            var user = result.Results?.FirstOrDefault();
            if (user == null)
            {
                throw new Exception("Get no user");
            }
            return user;
        }
    }
}
