import { Book } from './../../models/Book';
import { BaseApiHelper } from './BaseApiHelper';
class BookApiHelper extends BaseApiHelper {
  async getRecommended() : Promise<Array<Book> | null> { 
    try {
      const response = await this._axiosInstance.get<Array<Book>>("/books/recommended");

      return response.data;
    } catch (ex) {
      return null;
    }
  }
}

export const bookApiHelper = new BookApiHelper();