namespace miranaSolution.API.ViewModels.Books;

public record ApiGetBookRatingsOverviewResponse(
    Dictionary<int, int> RatingsByStar);