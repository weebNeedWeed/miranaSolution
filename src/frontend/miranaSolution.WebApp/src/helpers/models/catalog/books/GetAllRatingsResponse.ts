import {BookRating} from "./BookRating";

export interface GetAllRatingsResponse {
    bookRatings: BookRating[],
    pageIndex: number;
    pageSize: number;
    totalPages: number;
    totalRatings: number;
}