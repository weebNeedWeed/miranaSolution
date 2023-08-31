namespace miranaSolution.API.ViewModels.Books;

public record ApiGetBookRatingsOverviewResponse(
    int TotalRatings,
    int Avg,
    Dictionary<int, int> RatingsByStar);