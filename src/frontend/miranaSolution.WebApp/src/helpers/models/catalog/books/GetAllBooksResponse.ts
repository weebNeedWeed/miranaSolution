import {Book} from "./Book";

export interface GetAllBooksResponse {
    books: Array<Book>;
    pageIndex: number;
    pageSize: number;
    totalPages: number;
}