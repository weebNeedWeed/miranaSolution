export interface PagedResult<TData> {
  pageIndex: number;
  pageSize: number;
  totalRecords: number;
  totalPages: number;
  items: Array<TData>;
}