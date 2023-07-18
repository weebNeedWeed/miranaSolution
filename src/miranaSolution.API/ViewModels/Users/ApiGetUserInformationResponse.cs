namespace miranaSolution.API.ViewModels.Users;

public record ApiGetUserInformationResponse(
    Guid Id,
    string FirstName,
    string LastName,
    string UserName,
    string Email,
    string? Avatar, 
    int TotalComments);