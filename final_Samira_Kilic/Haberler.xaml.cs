using System.Collections.ObjectModel;
using final_Samira_Kilic.Model;
using final_Samira_Kilic.Services;

namespace final_Samira_Kilic;

public partial class Haberler : ContentPage
{
    public ObservableCollection<HaberlerItem> FilteredNewsItems { get; set; }

    public Haberler()
    {
        InitializeComponent();
        FilteredNewsItems = new ObservableCollection<HaberlerItem>();
        BindingContext = this;

        var defaultCategory = Kategori.CategoryList.First();
        _ = GetRoot(defaultCategory);
    }

    public async Task GetRoot(Kategori kategori)
    {
        try
        {
            var root = await HaberlerServis.GetNews(kategori);
            FilteredNewsItems.Clear();

            if (root != null)
            {
                foreach (var item in root.items)
                {
                    FilteredNewsItems.Add(new HaberlerItem
                    {
                        Title = item.title,
                        Author = item.author,
                        PubDate = item.pubDate,
                        Link = item.link,
                        ImageUrl = item.enclosure?.link
                    });
                }
            }
        }
        catch (Exception ex)
        {
            await DisplayAlert("Hata", "Haberler alınamadı: " + ex.Message, "Tamam");
        }
    }

    private async void CategoryTapped(object sender, EventArgs e)
    {
        if (sender is Label label && label.GestureRecognizers.FirstOrDefault() is TapGestureRecognizer tap)
        {
            var selected = tap.CommandParameter?.ToString();
            var category = Kategori.CategoryList.FirstOrDefault(k => k.Tittle == selected);
            if (category != null)
                await GetRoot(category);
        }
    }

    private async void NewsListView_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        if (e.CurrentSelection.FirstOrDefault() is HaberlerItem selectedNews)
        {
            await Navigation.PushAsync(new NewsDetailPage(selectedNews.Link));
            ((CollectionView)sender).SelectedItem = null;
        }
    }
}
