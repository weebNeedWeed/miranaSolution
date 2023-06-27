import { BaseApiHelper } from "./BaseApiHelper";
import { Genre } from "../models/catalog/books/Genre";
import { ApiResult } from "../models/ApiResult";

class GenreApiHelper extends BaseApiHelper {
  async getAll(): Promise<Genre[] | null> {
    try {
      const response = await this.init().get<ApiResult<Genre[]>>("/genres");
      if (response.data.status == "fail" || response.data.status == "error") {
        throw new Error();
      }

      return response.data.data;
    } catch (ex) {
      return null;
    }
  }
}

export const genreApiHelper = new GenreApiHelper();