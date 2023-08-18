using miranaSolution.DTOs.Core.Books;

namespace miranaSolution.Services.Core.Books;

public interface IBookService
{
    Task<GetBookByIdResponse> GetBookByIdAsync(GetBookByIdRequest request);

    Task<GetBookBySlugResponse> GetBookBySlugAsync(GetBookBySlugRequest request);

    Task<CreateBookResponse> CreateBookAsync(CreateBookRequest request);

    Task<UpdateBookResponse> UpdateBookAsync(UpdateBookRequest request);

    Task DeleteBookAsync(DeleteBookRequest request);

    Task<GetRecommendedBooksResponse> GetRecommendedBooksAsync();

    Task<GetAllBooksResponse> GetAllBooksAsync(GetAllBooksRequest request);

    Task<GetMostReadingBooksResponse> GetMostReadingBooks(GetMostReadingBooksRequest request);

    Task AssignGenresAsync(AssignGenresRequest request);
}