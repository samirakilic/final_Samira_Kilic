namespace final_Samira_Kilic.Model
{
    public class Root
    {
        public List<Item> items { get; set; }
    }

    public class Item
    {
        public string title { get; set; }
        public string pubDate { get; set; }
        public string author { get; set; }
        public string link { get; set; }
        public Enclosure enclosure { get; set; }
    }

    public class Enclosure
    {
        public string link { get; set; }
    }
}
