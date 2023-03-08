import { ApiResult } from '../../models/ApiResult';
import { Book } from './../../models/Book';
import { BaseApiHelper } from './BaseApiHelper';

class BookApiHelper extends BaseApiHelper {
  async getRecommended() : Promise<Array<Book> | null> { 
    try {
      const response = await this._axiosInstance.get<ApiResult<Array<Book>>>("/books/recommended");
      return response.data.data;
    } catch (ex) {
      return null;
    }
  }
}

export const bookApiHelper = new BookApiHelper();