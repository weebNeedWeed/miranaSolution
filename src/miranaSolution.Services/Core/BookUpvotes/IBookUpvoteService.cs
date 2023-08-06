using miranaSolution.DTOs.Core.BookUpvotes;

namespace miranaSolution.Services.Core.BookUpvotes;

public interface IBookUpvoteService
{
    Task<CreateBookUpvoteResponse> CreateBookUpvoteAsync(CreateBookUpvoteRequest request);

    Task DeleteBookUpvoteAsync(DeleteBookUpvoteRequest request);

    Task<CountBookUpvoteByBookIdResponse> CountBookUpvoteByBookIdAsync(CountBookUpvoteByBookIdRequest request);

    Task<CountBookUpvoteByUserIdResponse> CountBookUpvoteByUserIdAsync(CountBookUpvoteByUserIdRequest request);

    Task<GetAllBookUpvotesResponse> GetAllBookUpvotesAsync(GetAllBookUpvotesRequest request);
}