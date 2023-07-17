using miranaSolution.DTOs.Core.Authors;

namespace miranaSolution.Services.Core.Authors;

public interface IAuthorService
{
    Task<GetAllAuthorsResponse> GetAllAuthorsAsync();
    
    Task<CreateAuthorResponse> CreateAuthorAsync(CreateAuthorRequest request);

    Task DeleteAuthorAsync(DeleteAuthorRequest request);

    Task<UpdateAuthorResponse> UpdateAuthorAsync(UpdateAuthorRequest request);
}