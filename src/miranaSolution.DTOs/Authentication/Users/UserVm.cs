namespace miranaSolution.DTOs.Authentication.Users;

public record UserVm(
    Guid Id,
    string FirstName,
    string LastName,
    string UserName,
    string Email,
    string? Avatar,
    int ReadBookCount,
    int ReadChapterCount);