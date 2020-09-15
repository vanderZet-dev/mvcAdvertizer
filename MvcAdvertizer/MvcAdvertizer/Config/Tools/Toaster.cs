namespace MvcAdvertizer.Config.Tools
{
    public class Toaster
    {
        public string Header { get; set; }

        public string Text { get; set; }

        public string Color { get; set; }

        public bool Valid { get => Header != null && Text != null && Color != null; }
    }
}
