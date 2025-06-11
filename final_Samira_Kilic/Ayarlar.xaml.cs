namespace final_Samira_Kilic;

using Microsoft.Maui.Storage;

public partial class Ayarlar : ContentPage
{
    public Ayarlar()
    {
        InitializeComponent();

        // UserAppTheme de�eri null olabilir, bunu kontrol edelim
        var currentTheme = Application.Current.UserAppTheme;
        themeSwitch.IsToggled = currentTheme == AppTheme.Dark;
    }

    private void OnThemeToggled(object sender, ToggledEventArgs e)
    {
        string newTheme = e.Value ? "dark" : "light";
        App.ChangeTheme(newTheme);
    }
}


