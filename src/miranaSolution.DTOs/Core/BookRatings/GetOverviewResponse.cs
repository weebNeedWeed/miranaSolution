namespace miranaSolution.DTOs.Core.BookRatings;

public record GetOverviewResponse(
    Dictionary<int, int> RatingsByStar);