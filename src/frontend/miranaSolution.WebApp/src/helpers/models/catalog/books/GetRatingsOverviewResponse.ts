export interface GetRatingsOverviewResponse {
    ratingsByStar: {
        [index: number]: number
    }
}