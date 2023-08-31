namespace miranaSolution.DTOs.Core.BookRatings;

public record GetOverviewResponse(
    int TotalRatings,
    int Avg,
    Dictionary<int, int> RatingsByStar);