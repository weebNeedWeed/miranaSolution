import {BaseApiHelper} from "./BaseApiHelper";
import {Author} from "../models/catalog/author/Author";
import {ApiResult} from "../models/common/ApiResult";
import {GetAllAuthorsResponse} from "../models/catalog/author/GetAllAuthorsResponse";
import {Book} from "../models/catalog/books/Book";
import {GetAllBooksByAuthorIdResponse} from "../models/catalog/author/GetAllBooksByAuthorIdResponse";

class AuthorApiHelper extends BaseApiHelper {
    async getAllAuthors(): Promise<Author[]> {
        const response = await this.init().get<ApiResult<GetAllAuthorsResponse>>("/authors");
        if (response.data.status === "error") {
            throw new Error(response.data.message);
        }

        return response.data.data.authors;
    }

    async getAllBooksByAuthorId(authorId: number): Promise<Book[]> {
        const url = `/authors/${authorId}/books`;
        const response = await this.init().get<ApiResult<GetAllBooksByAuthorIdResponse>>(url);
        if (response.data.status === "error") {
            throw new Error(response.data.message);
        }

        return response.data.data.books;
    }
}

export const authorApiHelper = new AuthorApiHelper();