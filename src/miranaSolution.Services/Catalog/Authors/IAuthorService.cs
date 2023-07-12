using miranaSolution.DTOs.Catalog.Authors;

namespace miranaSolution.Services.Catalog.Authors;

public interface IAuthorService
{
    Task<GetAllAuthorsResponse> GetAllAuthorsAsync();
}