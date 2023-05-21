import { BaseApiClient } from "./BaseApiClient";
import { Author } from "../../models/Author";
import { ApiResult } from "../../models/ApiResult";

class AuthorsApiClient extends BaseApiClient {
  async GetAll(): Promise<Array<Author> | null> {
    try {
      const response = await this.init().get<ApiResult<Array<Author>>>("/authors");
      if (response.data.status === "fail" || response.data.status === "error") {
        return null;
      }

      return response.data.data;
    } catch (error) {
      return null;
    }
  }
}

export const authorsApiClient = new AuthorsApiClient();