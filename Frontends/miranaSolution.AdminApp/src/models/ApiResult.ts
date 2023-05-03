export interface ApiResult<TData> {
  status: "success" | "fail" | "error";
  data: TData;
  message: { [key: string]: Array<string> };
}
