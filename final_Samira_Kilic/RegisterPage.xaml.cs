using System;
using System.Text.RegularExpressions;
using Microsoft.Maui.Controls;
using Microsoft.Maui.Storage;

namespace final_Samira_Kilic
{
    public partial class RegisterPage : ContentPage
    {
        private readonly FirebaseService _firebaseService;
        private bool _isPasswordHidden = true;
        private bool _isConfirmPasswordHidden = true;

        public RegisterPage()
        {
            InitializeComponent();
            _firebaseService = new FirebaseService();

            // Başlangıçta şifre gizli olsun
            passwordEntry.IsPassword = true;
            confirmPasswordEntry.IsPassword = true;
        }

        private void TogglePasswordVisibility(object sender, EventArgs e)
        {
            _isPasswordHidden = !_isPasswordHidden;
            passwordEntry.IsPassword = _isPasswordHidden;
            togglePasswordBtn.Text = _isPasswordHidden ? "Göster" : "Gizle";
        }

        private void ToggleConfirmPasswordVisibility(object sender, EventArgs e)
        {
            _isConfirmPasswordHidden = !_isConfirmPasswordHidden;
            confirmPasswordEntry.IsPassword = _isConfirmPasswordHidden;
            toggleConfirmPasswordBtn.Text = _isConfirmPasswordHidden ? "Göster" : "Gizle";
        }

        private bool IsValidEmail(string email)
        {
            if (string.IsNullOrWhiteSpace(email))
                return false;

            // Basit email regex kontrolü
            return Regex.IsMatch(email, @"^[^@\s]+@[^@\s]+\.[^@\s]+$");
        }

        private async void OnRegisterClicked(object sender, EventArgs e)
        {
            var email = emailEntry.Text?.Trim();
            var password = passwordEntry.Text;
            var confirmPassword = confirmPasswordEntry.Text;

            if (string.IsNullOrWhiteSpace(email) || string.IsNullOrWhiteSpace(password))
            {
                await DisplayAlert("Hata", "Email ve şifre boş olamaz.", "Tamam");
                return;
            }

            if (!IsValidEmail(email))
            {
                await DisplayAlert("Hata", "Geçerli bir email adresi giriniz.", "Tamam");
                return;
            }

            if (password != confirmPassword)
            {
                await DisplayAlert("Hata", "Şifreler eşleşmiyor.", "Tamam");
                return;
            }

            registerBtn.IsEnabled = false; // Kayıt tuşunu disable et

            try
            {
                var firebaseResponse = await _firebaseService.RegisterUserAsync(email, password);

                if (firebaseResponse != null && !string.IsNullOrEmpty(firebaseResponse.IdToken))
                {
                    Preferences.Set("IdToken", firebaseResponse.IdToken);
                    Preferences.Set("UserEmail", firebaseResponse.Email);
                    Preferences.Set("UserId", firebaseResponse.LocalId);

                    await DisplayAlert("Başarılı", "Kayıt işlemi başarılı.", "Tamam");

                    await Navigation.PopAsync(); // Kayıt sonrası geri dön
                }
                else
                {
                    await DisplayAlert("Hata", "Kayıt sırasında bilinmeyen bir hata oluştu.", "Tamam");
                }
            }
            catch (Exception ex)
            {
                await DisplayAlert("Hata", ex.Message, "Tamam");
            }
            finally
            {
                registerBtn.IsEnabled = true; // Tuşu tekrar aç
            }
        }

        private async void OnGoToLoginClicked(object sender, EventArgs e)
        {
            await Navigation.PopAsync();
        }
    }
}
