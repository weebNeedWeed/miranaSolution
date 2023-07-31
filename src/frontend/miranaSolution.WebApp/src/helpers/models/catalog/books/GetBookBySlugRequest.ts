import {Book} from "./Book";

export interface GetBookBySlugRequest {
    book: Book;
    totalUpvotes: number;
    totalBookmarks: number;
}