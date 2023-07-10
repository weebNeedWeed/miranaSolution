namespace miranaSolution.DTOs.Common;

public class PagedResult<TItem> : PagedResultBase
{
    public List<TItem> Items { get; set; }
}