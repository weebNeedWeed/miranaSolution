import {ApiResult} from '../models/ApiResult';
import {Book} from '../models/catalog/books/Book';
import {BaseApiHelper} from './BaseApiHelper';

class BookApiHelper extends BaseApiHelper {
  async getRecommended(): Promise<Array<Book> | null> {
    try {
      const response = await this.init().get<ApiResult<Array<Book>>>("/books/recommended");
      return response.data.data;
    } catch (ex) {
      return null;
    }
  }
}

export const bookApiHelper = new BookApiHelper();