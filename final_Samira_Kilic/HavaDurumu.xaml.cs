using System;
using System.Collections.ObjectModel;
using System.IO;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.Maui.Controls;

namespace final_Samira_Kilic
{
    public partial class HavaDurumu : ContentPage
    {
        public ObservableCollection<SehirHavaDurumu> Sehirler { get; set; } = new();

        private readonly string dosyaYolu = Path.Combine(FileSystem.AppDataDirectory, "Sehirler.json");

        public HavaDurumu()
        {
            InitializeComponent();
            ImageCollection.ItemsSource = Sehirler;
            SehirleriYukleAsync();
        }

        private async void OnButtonClick(object sender, EventArgs e)
        {
            string? sehir = await DisplayPromptAsync("Şehir", "Şehir ismi:", "OK", "Cancel");

            if (!string.IsNullOrWhiteSpace(sehir))
            {
                sehir = SehirIsimGuncelle(sehir);
                if (Sehirler.Any(s => s.Name == sehir))
                {
                    await DisplayAlert("Uyarı", $"{sehir} zaten listede var.", "Tamam");
                    return;
                }

                var yeniSehir = new SehirHavaDurumu { Name = sehir };
                Sehirler.Add(yeniSehir);
                await SehirleriKaydetAsync();
                await DisplayAlert("Şehir Eklendi", $"{sehir} şehri eklendi.", "Tamam");
            }
            else
            {
                await DisplayAlert("Hata", "Geçerli bir şehir adı giriniz.", "Tamam");
            }
        }

        private async void OnDeleteClicked(object sender, EventArgs e)
        {
            if ((sender as Button)?.BindingContext is SehirHavaDurumu silinecek)
            {
                bool onay = await DisplayAlert("Şehir Sil", $"{silinecek.Name} şehri silinsin mi?", "Evet", "Hayır");
                if (onay)
                {
                    Sehirler.Remove(silinecek);
                    await SehirleriKaydetAsync();
                }
            }
        }

        private async void OnRefreshButtonClick(object sender, EventArgs e)
        {
            // Yenile işlevi (örnek olarak şimdilik uyarı)
            await DisplayAlert("Yenile", "Hava durumu verileri yenilendi.", "Tamam");
        }

        private string SehirIsimGuncelle(string sehir)
        {
            sehir = sehir.ToUpper(System.Globalization.CultureInfo.CurrentCulture);
            return sehir.Replace('Ç', 'C')
                        .Replace('Ğ', 'G')
                        .Replace('İ', 'I')
                        .Replace('Ö', 'O')
                        .Replace('Ü', 'U')
                        .Replace('Ş', 'S');
        }

        private async Task SehirleriKaydetAsync()
        {
            try
            {
                var json = JsonSerializer.Serialize(Sehirler);
                await File.WriteAllTextAsync(dosyaYolu, json);
            }
            catch (Exception ex)
            {
                await DisplayAlert("Hata", "Şehirleri kaydederken hata oluştu: " + ex.Message, "Tamam");
            }
        }

        private async void SehirleriYukleAsync()
        {
            try
            {
                if (File.Exists(dosyaYolu))
                {
                    var json = await File.ReadAllTextAsync(dosyaYolu);
                    var sehirListesi = JsonSerializer.Deserialize<ObservableCollection<SehirHavaDurumu>>(json);
                    if (sehirListesi != null)
                    {
                        Sehirler.Clear();
                        foreach (var sehir in sehirListesi)
                            Sehirler.Add(sehir);
                    }
                }
            }
            catch (Exception ex)
            {
                await DisplayAlert("Hata", "Şehirleri yüklerken hata oluştu: " + ex.Message, "Tamam");
            }
        }

        private async void ImageCollection_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (e.CurrentSelection.FirstOrDefault() is SehirHavaDurumu selectedSehir)
            {
                await DisplayAlert("Seçilen Şehir", $"Seçilen Şehir: {selectedSehir.Name}", "Tamam");
            }
            ((CollectionView)sender).SelectedItem = null;
        }
    }
}
