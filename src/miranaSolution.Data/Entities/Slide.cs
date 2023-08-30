namespace miranaSolution.Data.Entities;

public class Slide
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string ShortDescription { get; set; } = string.Empty;
    public string ThumbnailImage { get; set; } = string.Empty;
    public string Genres { get; set; } = string.Empty;
    public string Url { get; set; } = string.Empty;
    public int SortOrder { get; set; }
}