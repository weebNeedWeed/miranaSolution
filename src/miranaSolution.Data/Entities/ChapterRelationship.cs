namespace miranaSolution.Data.Entities;

public class ChapterRelationship
{
    public int FromId { get; set; }
    
    public Chapter? From { get; set; }
    
    public int ToId { get; set; }
    
    public Chapter? To { get; set; }
}