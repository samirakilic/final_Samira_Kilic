using System;
using Microsoft.Maui.Controls;
using Microsoft.Maui.Storage;

namespace final_Samira_Kilic
{
    public partial class LoginPage : ContentPage
    {
        private readonly FirebaseService _firebaseService;

        public LoginPage()
        {
            InitializeComponent();
            _firebaseService = new FirebaseService();
        }

        private async void OnLoginClicked(object sender, EventArgs e)
        {
            var email = emailEntry.Text?.Trim();
            var password = passwordEntry.Text?.Trim();

            if (string.IsNullOrEmpty(email) || string.IsNullOrEmpty(password))
            {
                await DisplayAlert("Eksik Bilgi", "Lütfen email ve şifreyi doldurun.", "Tamam");
                return;
            }

            try
            {
                var response = await _firebaseService.LoginUserAsync(email, password);

                // Token ve UserId'yi Preferences'a kaydet
                Preferences.Set("IdToken", response.IdToken);
                Preferences.Set("UserId", response.LocalId);

                await DisplayAlert("Başarılı", "Giriş başarılı", "Tamam");

                // Ana sayfaya yönlendir
                Application.Current.MainPage = new AppShell();
            }
            catch (Exception ex)
            {
                // Firebase'den gelen hata mesajını kullanıcı dostu yapabilirsin
                await DisplayAlert("Hata", ex.Message, "Tamam");
            }
        }

        private async void OnRegisterClicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new RegisterPage());
        }
    }
}
