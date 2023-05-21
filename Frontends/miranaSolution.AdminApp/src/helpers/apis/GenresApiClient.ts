import { BaseApiClient } from "./BaseApiClient";
import { Author } from "../../models/Author";
import { ApiResult } from "../../models/ApiResult";
import { Genre } from "../../models/Genre";

class GenresApiClient extends BaseApiClient {
  async GetAll(): Promise<Array<Genre> | null> {
    try {
      const response = await this.init().get<ApiResult<Array<Genre>>>("/genres");
      if (response.data.status === "fail" || response.data.status === "error") {
        return null;
      }

      return response.data.data;
    } catch (error) {
      return null;
    }
  }
}

export const genresApiClient = new GenresApiClient();