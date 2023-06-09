namespace miranaSolution.Dtos.Common;

public class PagedResultBase
{
    public int PageIndex { get; set; }
    public int PageSize { get; set; }
    public int TotalRecords { get; set; }

    public int TotalPages
    {
        get
        {
            return (int)Math.Ceiling(TotalRecords / (double)PageSize);
        }
    }
}