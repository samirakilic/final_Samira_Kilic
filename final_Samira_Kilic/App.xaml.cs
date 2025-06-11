namespace final_Samira_Kilic;

public partial class App : Application
{
    public App()
    {
        InitializeComponent();

        MainPage = new NavigationPage(new LoginPage());
    }

    // Tema değiştirme fonksiyonu
    public static void ChangeTheme(string theme)
    {
        if (theme == "dark")
            Application.Current.UserAppTheme = AppTheme.Dark;
        else
            Application.Current.UserAppTheme = AppTheme.Light;
    }
}