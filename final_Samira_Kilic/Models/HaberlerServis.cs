using System.Net.Http.Json;
using final_Samira_Kilic.Model;

namespace final_Samira_Kilic.Services
{
    public class HaberlerServis
    {
        public static async Task<Root> GetNews(Kategori kategori)
        {
            using var client = new HttpClient();
            string apiUrl = $"https://api.rss2json.com/v1/api.json?rss_url={kategori.Link}";
            return await client.GetFromJsonAsync<Root>(apiUrl);
        }
    }
}
