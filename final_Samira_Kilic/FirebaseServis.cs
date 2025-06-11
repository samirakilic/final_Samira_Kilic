using System;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Microsoft.Maui.Storage; // Preferences için

namespace final_Samira_Kilic
{
    public class FirebaseService
    {
        private readonly string firebaseApiKey = "AIzaSyBTBZ3MVgl9ZlWwjy7o7t6kOO_WMc0Y9no";
        private readonly string signUpUrl = "https://identitytoolkit.googleapis.com/v1/accounts:signUp?key=";
        private readonly string signInUrl = "https://identitytoolkit.googleapis.com/v1/accounts:signInWithPassword?key=";

        private readonly HttpClient _httpClient;

        public FirebaseService()
        {
            _httpClient = new HttpClient();
        }

        public async Task<FirebaseResponse> RegisterUserAsync(string email, string password)
        {
            var requestBody = new
            {
                email,
                password,
                returnSecureToken = true
            };

            var content = new StringContent(JsonConvert.SerializeObject(requestBody), Encoding.UTF8, "application/json");
            var response = await _httpClient.PostAsync(signUpUrl + firebaseApiKey, content);

            var result = await response.Content.ReadAsStringAsync();

            if (response.IsSuccessStatusCode)
            {
                return JsonConvert.DeserializeObject<FirebaseResponse>(result);
            }
            else
            {
                var errorResponse = JsonConvert.DeserializeObject<FirebaseErrorResponse>(result);
                throw new Exception(errorResponse?.Error?.Message ?? "Kayıt başarısız");
            }
        }

        public async Task<FirebaseResponse> LoginUserAsync(string email, string password)
        {
            var requestBody = new
            {
                email,
                password,
                returnSecureToken = true
            };

            var content = new StringContent(JsonConvert.SerializeObject(requestBody), Encoding.UTF8, "application/json");
            var response = await _httpClient.PostAsync(signInUrl + firebaseApiKey, content);

            var result = await response.Content.ReadAsStringAsync();

            if (response.IsSuccessStatusCode)
            {
                return JsonConvert.DeserializeObject<FirebaseResponse>(result);
            }
            else
            {
                var errorResponse = JsonConvert.DeserializeObject<FirebaseErrorResponse>(result);
                throw new Exception(errorResponse?.Error?.Message ?? "Giriş başarısız");
            }
        }
    }

    public class FirebaseResponse
    {
        [JsonProperty("idToken")]
        public string IdToken { get; set; }

        [JsonProperty("refreshToken")]
        public string RefreshToken { get; set; }

        [JsonProperty("localId")]
        public string LocalId { get; set; }

        [JsonProperty("email")]
        public string Email { get; set; }
    }

    public class FirebaseErrorResponse
    {
        public FirebaseError Error { get; set; }
    }

    public class FirebaseError
    {
        public string Message { get; set; }
    }
}