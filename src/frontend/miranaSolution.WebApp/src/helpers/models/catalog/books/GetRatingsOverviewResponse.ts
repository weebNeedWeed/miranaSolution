export interface GetRatingsOverviewResponse {
    totalRatings: number;
    avg: number;
    ratingsByStar: {
        [index: number]: number
    }
}