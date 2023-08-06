namespace miranaSolution.DTOs.Core.CurrentlyReading;

public record RemoveBookRequest(
    Guid UserId,
    int BookId);