using miranaSolution.Dtos.Common;

namespace miranaSolution.Dtos.Catalog.Books
{
    public class BookGetPagingRequest : PagingRequestBase
    {
        public string Keyword { get; set; } = string.Empty;
        
        /// <summary>
        /// String of list of selected id matching the pattern: "id1,id2,...,idn"
        /// </summary>
        public string GenreIds { get; set; } = string.Empty;
        
        public bool? IsDone { get; set; }
    }
}