using miranaSolution.Dtos.Catalog.Authors;

namespace miranaSolution.Business.Catalog.Authors;

public interface IAuthorService
{
    Task<List<AuthorDto>> GetAll();
    // Task<AuthorDto> Create();
    // Task Delete();
    // Task Update();
}