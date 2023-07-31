using miranaSolution.DTOs.Authentication.Users;

namespace miranaSolution.API.ViewModels.Users;

public record ApiGetUserProfileResponse(
    UserVm User,
    int TotalComments,
    int TotalReactions,
    int TotalBookmarks,
    int TotalUpvotes,
    int TotalRatings);