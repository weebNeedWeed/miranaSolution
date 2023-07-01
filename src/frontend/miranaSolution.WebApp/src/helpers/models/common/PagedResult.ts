export interface PagedResult<TData> {
    items: Array<TData>;
    pageIndex: number;
    pageSize: number;
    totalPages: number;
    totalRecords: number;
}