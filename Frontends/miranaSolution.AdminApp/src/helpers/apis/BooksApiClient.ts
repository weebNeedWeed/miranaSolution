import { BaseApiClient } from "./BaseApiClient";
import { Book } from "../../models/Book";
import { ApiResult } from "../../models/ApiResult";
import { PagedResult } from "../../models/PagedResult";

class BooksApiClient extends BaseApiClient {
  async getPaging(pageIndex: number = 1, pageSize: number = 10, keyword: string = ""): Promise<PagedResult<Book> | null> {
    const params = new URLSearchParams();
    params.append("pageIndex", pageIndex.toString());
    params.append("pageSize", pageSize.toString());
    params.append("keyword", keyword);

    const queryString = `/books?${params.toString()}`;

    try {
      const response = await this.init().get<ApiResult<PagedResult<Book>>>(queryString);
      if (response.data.status === "error" || response.data.status === "fail") {
        return null;
      }

      return response.data.data;
    } catch (err) {
      return null;
    }
  }

  async create(formData: FormData): Promise<Book | null> {
    try {
      const response = await this.init().post<ApiResult<Book>>("/books", formData);
      if (response.data.status === "error" || response.data.status === "fail") {
        return null;
      }

      return response.data.data;
    } catch (ex) {
      return null;
    }
  }
}

export const booksApiClient = new BooksApiClient();