using miranaSolution.DTOs.Catalog.Books;

namespace miranaSolution.Services.Catalog.Books;

public interface IBookService
{
    Task<GetBookByIdResponse> GetBookByIdAsync(GetBookByIdRequest request);

    Task<GetBookBySlugResponse> GetBookBySlugAsync(GetBookBySlugRequest request);
    
    Task<CreateBookResponse> CreateBookAsync(CreateBookRequest request);
    
    Task<UpdateBookResponse> UpdateBookAsync(UpdateBookRequest request);

    Task DeleteBookAsync(DeleteBookRequest request);

    Task<GetRecommendedBooksResponse> GetRecommendedBooksAsync();

    Task<GetAllBooksResponse> GetAllBooksAsync(GetAllBooksRequest request);
}