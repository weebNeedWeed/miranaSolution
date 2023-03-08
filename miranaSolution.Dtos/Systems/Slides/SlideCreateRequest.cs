namespace miranaSolution.Dtos.Systems.Slides
{
    public class SlideCreateRequest
    {
        public string Name { get; set; }
        public string ShortDescription { get; set; }
        public string ThumbnailImage { get; set; }
        public string Genres { get; set; }
        public int SortOrder { get; set; }
    }
}