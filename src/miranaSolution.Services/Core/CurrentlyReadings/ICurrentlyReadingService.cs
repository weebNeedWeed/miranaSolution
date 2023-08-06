using miranaSolution.DTOs.Core.CurrentlyReading;

namespace miranaSolution.Services.Core.CurrentlyReadings;

public interface ICurrentlyReadingService
{
    Task<AddBookResponse> AddBookAsync(AddBookRequest request);

    Task RemoveBookAsync(RemoveBookRequest request);

    Task<GetCurrentlyReadingBooksResponse> GetCurrentlyReadingBooksAsync(GetCurrentlyReadingBooksRequest request);
}