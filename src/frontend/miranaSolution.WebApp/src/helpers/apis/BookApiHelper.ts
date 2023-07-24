import {ApiResult} from '../models/common/ApiResult';
import {Book} from '../models/catalog/books/Book';
import {BaseApiHelper} from './BaseApiHelper';
import {Chapter} from "../models/catalog/books/Chapter";
import {GetAllBooksResponse} from "../models/catalog/books/GetAllBooksResponse";
import {GetAllBooksRequest} from "../models/catalog/books/GetAllBooksRequest";
import {GetRecommendedBooksResponse} from "../models/catalog/books/GetRecommendedBooksResponse";
import {GetLatestChaptersResponse} from "../models/catalog/books/GetLatestChaptersResponse";
import {GetBookBySlugRequest} from "../models/catalog/books/GetBookBySlugRequest";
import {GetMostReadingBooksResponse} from "../models/catalog/books/GetMostReadingBooksResponse";
import {GetAllChaptersRequest} from "../models/catalog/books/GetAllChaptersRequest";
import {GetAllChaptersResponse} from "../models/catalog/books/GetAllChaptersResponse";

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

        if (response.data.status === "error") {
            throw new Error(response.data.message);
        }

        return response.data.data;
    }

    async getRecommendedBooks(): Promise<Array<Book>> {
        const response = await this.init()
            .get<ApiResult<GetRecommendedBooksResponse>>("/books/recommended");

        if (response.data.status === "error") {
            throw new Error(response.data.message);
        }

        return response.data.data.books;
    }

    async getBookBySlug(slug: string): Promise<GetBookBySlugRequest> {
        const url = `/books/${encodeURIComponent(slug)}`;

        const response = await this.init().get<ApiResult<GetBookBySlugRequest>>(url);
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

    async getLatestChapters(numberOfChapters: number = 10): Promise<Chapter[]> {
        const url = `/books/chapters/latest?numberOfChapters=${numberOfChapters}`;

        const response = await this.init().get<ApiResult<GetLatestChaptersResponse>>(url);
        if (response.data.status === "error") {
            throw new Error(response.data.message);
        }

        return response.data.data.chapters;
    }

    async getBookById(bookId: number): Promise<Book> {
        const url = `/books/${bookId}`;

        const response = await this.init().get<ApiResult<Book>>(url);
        if (response.data.status === "error") {
            throw new Error(response.data.message);
        }

        return response.data.data;
    }

    async getMostReadingBooks(numberOfBooks: number = 12): Promise<GetMostReadingBooksResponse> {
        const params = new URLSearchParams();
        params.append("numberOfBooks", numberOfBooks.toString());
        const url = `/books/most_reading?${params.toString()}`;

        const response = await this.init().get<ApiResult<GetMostReadingBooksResponse>>(url);
        if (response.data.status === "error") {
            throw new Error(response.data.message);
        }

        return response.data.data;
    }

    async getAllChapters(request: GetAllChaptersRequest): Promise<GetAllChaptersResponse> {
        const params = new URLSearchParams();
        params.append("pageIndex", request.pageIndex.toString());
        params.append("pageSize", request.pageSize.toString());

        const url = `/books/${request.bookId}/chapters?${params.toString()}`;

        const response = await this.init().get<ApiResult<GetAllChaptersResponse>>(url);
        if (response.data.status === "error") {
            throw new Error(response.data.message);
        }

        return response.data.data;
    }
}

export const bookApiHelper = new BookApiHelper();