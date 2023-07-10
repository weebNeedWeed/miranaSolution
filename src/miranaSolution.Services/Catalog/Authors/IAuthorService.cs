using miranaSolution.DTOs.Catalog.Authors;

namespace miranaSolution.Services.Catalog.Authors;

public interface IAuthorService
{
    Task<List<AuthorDto>> GetAll();
    // Task<AuthorDto> Create();
    // Task Delete();
    // Task Update();
}