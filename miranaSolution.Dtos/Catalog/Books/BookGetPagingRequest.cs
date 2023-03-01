namespace miranaSolution.Dtos.Catalog.Books
{
    public class BookGetPagingRequest
    {
        public int PageIndex { get; set; }
        public int PageSize { get; set; }
        public string? Keyword { get; set; }
    }
}