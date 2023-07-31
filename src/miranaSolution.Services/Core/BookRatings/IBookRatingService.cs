using miranaSolution.DTOs.Core.BookRatings;
using miranaSolution.DTOs.Core.Books;

namespace miranaSolution.Services.Core.BookRatings;

public interface IBookRatingService
{
    Task<CreateBookRatingResponse> CreateBookRatingAsync(CreateBookRatingRequest request);

    Task DeleteBookRatingAsync(DeleteBookRatingRequest request);

    Task<UpdateBookRatingResponse> UpdateBookRatingAsync(UpdateBookRatingRequest request);

    Task<GetAllBookRatingsByUserIdResponse> GetAllBookRatingsByUserIdAsync(GetAllBookRatingsByUserIdRequest request);

    Task<GetAllBookRatingsByBookIdResponse> GetAllBookRatingsByBookIdAsync(GetAllBookRatingsByBookIdRequest request);

    Task<CheckUserIsRatedResponse> CheckUserIsRatedAsync(CheckUserIsRatedRequest request);
}