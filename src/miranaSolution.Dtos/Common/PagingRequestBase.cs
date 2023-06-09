using System.ComponentModel.DataAnnotations;

namespace miranaSolution.Dtos.Common;

public class PagingRequestBase
{
    public int PageIndex { get; set; }
    public int PageSize { get; set; }
    public string? Keyword { get; set; }
}