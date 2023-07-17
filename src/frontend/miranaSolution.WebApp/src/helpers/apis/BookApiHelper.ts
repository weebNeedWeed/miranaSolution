import {ApiResult} from '../models/common/ApiResult';
import {Book} from '../models/catalog/books/Book';
import {BaseApiHelper} from './BaseApiHelper';
import {Chapter} from "../models/catalog/books/Chapter";
import {GetAllBooksResponse} from "../models/catalog/books/GetAllBooksResponse";
import {GetAllBooksRequest} from "../models/catalog/books/GetAllBooksRequest";

class BookApiHelper extends BaseApiHelper {
    async getAllBooks(request: GetAllBooksRequest): Promise<GetAllBooksResponse> {
        const params = new URLSearchParams();
        params.append("pageIndex", request.pageIndex.toString());
        params.append("pageSize", request.pageSize.toString());

        if (request.keyword != null) {
            params.append("keyword", request.keyword!);
        }

        if (request.genreIds != null) {
            params.append("genreIds", request.genreIds!);
        }

        if (request.isDone != null) {
            params.append("isDone", request.isDone ? "true" : "false");
        }

        const response = await this.init()
            .get<ApiResult<GetAllBooksResponse>>("/books?" + params.toString());

        return response.data.data;
    }

    async getRecommendedBooks(): Promise<Array<Book>> {
        const response = await this.init()
            .get<ApiResult<Array<Book>>>("/books/recommended");
        return response.data.data;
    }

    async getBookBySlug(slug: string): Promise<Book> {
        const url = `/books/${encodeURIComponent(slug)}`;

        const response = await this.init().get<ApiResult<Book>>(url);
        if (response.data.status === "error") {
            throw new Error(response.data.message);
        }

        return response.data.data;
    }

    async getBookChapterByIndex(bookId: number, index: number): Promise<Chapter> {
        const url = `/books/${bookId}/chapters/${index}`;

        const response = await this.init().get<ApiResult<Chapter>>(url);
        if (response.data.status === "error") {
            throw new Error(response.data.message);
        }

        return response.data.data;
    }
}

export const bookApiHelper = new BookApiHelper();