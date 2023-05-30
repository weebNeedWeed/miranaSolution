export interface ApiResult<TData> {
  status: "success" | "fail" | "error";
  data: TData;
  message: string;
}