export interface GetAllCommentsRequest {
    bookId: number,
    pageIndex: number,
    pageSize: number,
    parentId?: number,
    asc?: boolean,
}