namespace final_Samira_Kilic.Model
{
    public class Kategori
    {
        public string Tittle { get; set; }
        public string Link { get; set; }

        public static List<Kategori> CategoryList = new List<Kategori>
        {
            new Kategori { Tittle = "Manşet", Link = "https://www.trthaber.com/manset_articles.rss" },
            new Kategori { Tittle = "Ekonomi", Link = "https://www.trthaber.com/ekonomi_articles.rss" },
            new Kategori { Tittle = "Gündem", Link = "https://www.trthaber.com/gundem_articles.rss" },
            new Kategori { Tittle = "Spor", Link = "https://www.trthaber.com/spor_articles.rss" },
            new Kategori { Tittle = "Bilim", Link = "https://www.trthaber.com/bilim_teknoloji_articles.rss" }
        };
    }
}
