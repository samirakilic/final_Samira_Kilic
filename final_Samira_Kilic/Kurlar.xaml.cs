using Newtonsoft.Json;
using System.Net.Http;

namespace final_Samira_Kilic;

public partial class Kurlar : ContentPage
{
    private AltinDoviz kurlar;

    public Kurlar()
    {
        InitializeComponent();
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();
        await YükleAsync();
    }

    private async void OnRefreshClicked(object sender, EventArgs e)
    {
        await YükleAsync();
    }

    private async Task YükleAsync()
    {
        try
        {
            loadingIndicator.IsVisible = true;
            loadingIndicator.IsRunning = true;
            dovizliste.IsVisible = false;

            string jsonData = await GetKurlarJsonAsync();

            kurlar = JsonConvert.DeserializeObject<AltinDoviz>(jsonData);

            dovizliste.ItemsSource = new List<KurItem>()
            {
                new KurItem("Dolar", kurlar.USD),
                new KurItem("Euro", kurlar.EUR),
                new KurItem("Suudi Riyali", kurlar.SAR),
                new KurItem("Katar Riyali", kurlar.QAR),
                new KurItem("Mısır Lirası", kurlar.EGP),
                new KurItem("Japon Yeni", kurlar.JPY),
                new KurItem("Hindistan Rupisi", kurlar.INR),
                new KurItem("Avustralya Doları", kurlar.AUD),
                new KurItem("Kanada Doları", kurlar.CAD),
                new KurItem("İsviçre Frangı", kurlar.CHF),
                new KurItem("Singapur Doları", kurlar.SGD),
            };
        }
        catch (Exception ex)
        {
            await DisplayAlert("Hata", $"Kurlar yüklenemedi: {ex.Message}", "Tamam");
        }
        finally
        {
            loadingIndicator.IsRunning = false;
            loadingIndicator.IsVisible = false;
            dovizliste.IsVisible = true;
        }
    }

    private string GetImage(string degisim)
    {
        return degisim.Contains("-") ? "down.png" : "up.png";
    }

    private async Task<string> GetKurlarJsonAsync()
    {
        using var client = new HttpClient();
        string url = "https://finans.truncgil.com/today.json";

        var response = await client.GetAsync(url);
        response.EnsureSuccessStatusCode();

        return await response.Content.ReadAsStringAsync();
    }
}

// Model Sınıfları

public class KurItem
{
    public string Doviz { get; set; }
    public string Alis { get; set; }
    public string Satis { get; set; }
    public string Fark { get; set; }
    public string Yon { get; set; }

    public KurItem(string doviz, Currency currency)
    {
        Doviz = doviz;
        Alis = currency.Alis;
        Satis = currency.Satis;
        Fark = currency.Degisim;
        Yon = currency.Degisim.Contains("-") ? "down.png" : "up.png";
    }
}

public class AltinDoviz
{
    public string Update_Date { get; set; }
    public Currency USD { get; set; }
    public Currency EUR { get; set; }
    public Currency SAR { get; set; }
    public Currency QAR { get; set; }
    public Currency EGP { get; set; }
    public Currency JPY { get; set; }
    public Currency INR { get; set; }
    public Currency AUD { get; set; }
    public Currency CAD { get; set; }
    public Currency CHF { get; set; }
    public Currency SGD { get; set; }
}

public class Currency
{
    [JsonProperty("Alış")]
    public string Alis { get; set; }

    [JsonProperty("Satış")]
    public string Satis { get; set; }

    [JsonProperty("Değişim")]
    public string Degisim { get; set; }
}
