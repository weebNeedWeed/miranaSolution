export interface BookGetPagingRequest {
    pageIndex: number;
    pageSize: number;
    keyword?: string;
    genreIds?: string;
    isDone?: boolean;
}