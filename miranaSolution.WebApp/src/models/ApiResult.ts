export interface ApiResult<TData> {
  isSucceed: boolean;
  data: TData;
  message: string;
}