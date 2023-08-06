export interface GetAllBooksRequest {
    pageIndex: number;
    pageSize: number;
    keyword?: string;
    genreIds?: string;
    isDone?: boolean;
    author?: number;
}