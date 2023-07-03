import {ApiResult} from '../models/ApiResult';
import {Book} from '../models/catalog/books/Book';
import {BaseApiHelper} from './BaseApiHelper';
import {Chapter} from "../models/catalog/books/Chapter";
import {BookGetPagingRequest} from "../models/catalog/books/BookGetPagingRequest";
import {PagedResult} from "../models/common/PagedResult";

class BookApiHelper extends BaseApiHelper {
    async getPaging(request: BookGetPagingRequest): Promise<PagedResult<Book> | null> {
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
        
        try {
            const response = await this.init().get<ApiResult<PagedResult<Book>>>("/books?" + params.toString());
            if (response.data.status === "fail" || response.data.status === "error") {
                throw new Error();
            }

            return response.data.data;
        } catch (ex) {
            return null;
        }
    }

    async getRecommended(): Promise<Array<Book> | null> {
        try {
            const response = await this.init().get<ApiResult<Array<Book>>>("/books/recommended");
            return response.data.data;
        } catch (ex) {
            return null;
        }
    }

    async getBookBySlug(slug: string): Promise<Book | null> {
        const url = `/books/${encodeURIComponent(slug)}`;

        try {
            const response = await this.init().get<ApiResult<Book>>(url);
            if (response.data.status === "error" || response.data.status === "fail") {
                throw new Error();
            }

            return response.data.data;
        } catch (ex) {
            return null;
        }
    }

    async getChapterByIndex(id: number, index: number): Promise<Chapter | null> {
        const url = `/books/${id}/chapters/${index}`;

        try {
            const response = await this.init().get<ApiResult<Chapter>>(url);
            if (response.data.status === "error" || response.data.status === "fail") {
                throw new Error();
            }

            return response.data.data;
        } catch (ex) {
            return null;
        }
    }
}

export const bookApiHelper = new BookApiHelper();