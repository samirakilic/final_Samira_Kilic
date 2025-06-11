using Microsoft.Maui.ApplicationModel;

namespace final_Samira_Kilic;

public partial class NewsDetailPage : ContentPage
{
    private readonly string _newsUrl;

    public NewsDetailPage(string url)
    {
        InitializeComponent();
        _newsUrl = url;
        webView.Source = _newsUrl;
    }

    private async void ShareNews_Clicked(object sender, EventArgs e)
    {
        if (!string.IsNullOrWhiteSpace(_newsUrl))
        {
            await Share.Default.RequestAsync(new ShareTextRequest
            {
                Uri = _newsUrl,
                Title = "Haberi Paylaş"
            });
        }
        else
        {
            await DisplayAlert("Hata", "Paylaşılacak bağlantı bulunamadı.", "Tamam");
        }
    }
}
