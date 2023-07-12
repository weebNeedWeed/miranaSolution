namespace miranaSolution.DTOs.Common;

public record PagerResponse(
    int PageIndex,
    int PageSize,
    int TotalRecords)
{
    public int TotalPages => (int)Math.Ceiling(TotalRecords / (double)PageSize);
}