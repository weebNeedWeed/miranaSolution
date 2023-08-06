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
import {GetAllCommentsRequest} from "../models/catalog/books/GetAllCommentsRequest";
import {GetAllCommentsResponse} from "../models/catalog/books/GetAllCommentsResponse";
import {CreateCommentRequest} from "../models/catalog/books/CreateCommentRequest";
import {Comment} from "../models/catalog/comment/Comment";
import {ValidationFailureMessages} from "../models/common/ValidationFailureMessages";
import {DeleteCommentRequest} from "../models/catalog/books/DeleteCommentRequest";
import {BookRating} from "../models/catalog/books/BookRating";
import {CreateRatingRequest} from "../models/catalog/books/CreateRatingRequest";
import {GetAllRatingsResponse} from "../models/catalog/books/GetAllRatingsResponse";
import {UpdateRatingRequest} from "../models/catalog/books/UpdateRatingRequest";
import {BookUpvote} from "../models/catalog/books/BookUpvote";
import {GetAllUpvotesResponse} from "../models/catalog/books/GetAllUpvotesResponse";
import {GetRatingsOverviewResponse} from "../models/catalog/books/GetRatingsOverviewResponse";

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

        if (request.author != null) {
            params.append("authorId", request.author!.toString());
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

    async getAllComments(request: GetAllCommentsRequest): Promise<GetAllCommentsResponse> {
        const params = new URLSearchParams();
        params.append("pageIndex", request.pageIndex.toString());
        params.append("pageSize", request.pageSize.toString());

        if (typeof request.parentId !== "undefined") {
            params.append("parentId", request.parentId.toString());
        }

        if (typeof request.asc !== "undefined") {
            params.append("asc", request.asc ? "true" : "false");
        }

        const url = `/books/${request.bookId}/comments?${params.toString()}`;

        const response = await this.init().get<ApiResult<GetAllCommentsResponse>>(url);
        if (response.data.status === "error") {
            throw new Error(response.data.message);
        }

        return response.data.data;
    }

    async createComment(accessToken: string, bookId: number, request: CreateCommentRequest): Promise<Comment> {
        const headers = {
            Authorization: `Bearer ${accessToken}`,
        }

        const url = `/books/${bookId}/comments`;
        const response = await this.init(headers)
            .post<ApiResult<Comment | ValidationFailureMessages<CreateCommentRequest>>>(url, request);
        if (response.data.status === "error") {
            throw new Error(response.data.message);
        }

        if (response.data.status === "fail") {
            const failureMessages = response.data.data as ValidationFailureMessages<CreateCommentRequest>;
            throw new Error(Object.values(failureMessages)[0]);
        }

        return response.data.data as Comment;
    }

    async deleteComment(accessToken: string, request: DeleteCommentRequest): Promise<void> {
        const headers = {
            Authorization: `Bearer ${accessToken}`,
        }

        const url = `/books/${request.bookId}/comments/${request.commentId}`;
        const response = await this.init(headers)
            .delete<ApiResult<unknown>>(url);
        if (response.data.status === "error") {
            throw new Error(response.data.message);
        }
    }

    async createRating(accessToken: string, bookId: number, request: CreateRatingRequest): Promise<BookRating> {
        const headers = {
            Authorization: `Bearer ${accessToken}`,
        }

        const url = `/books/${bookId}/ratings`;
        const response = await this.init(headers)
            .post<ApiResult<BookRating>>(url, request);

        if (response.data.status === "error") {
            throw new Error(response.data.message);
        }

        return response.data.data;
    }

    async getAllRatings(bookId: number, userId?: string, pageIndex = 1, pageSize = 10): Promise<GetAllRatingsResponse> {
        const queryString = new URLSearchParams();
        if (typeof userId !== "undefined") {
            queryString.append("userId", userId);
        }

        queryString.append("pageIndex", pageIndex.toString());
        queryString.append("pageSize", pageSize.toString());

        let url = `/books/${bookId}/ratings?${queryString.toString()}`;

        const response = await this.init()
            .get<ApiResult<GetAllRatingsResponse>>(url);
        if (response.data.status === "error") {
            throw new Error(response.data.message);
        }

        return response.data.data;
    }

    async deleteRating(accessToken: string, bookId: number): Promise<void> {
        const headers = {
            Authorization: `Bearer ${accessToken}`,
        }

        const url = `/books/${bookId}/ratings`;
        const response = await this.init(headers)
            .delete<ApiResult<unknown>>(url);
        if (response.data.status === "error") {
            throw new Error(response.data.message);
        }
    }

    async updateRating(accessToken: string, bookId: number, request: UpdateRatingRequest): Promise<BookRating> {
        const headers = {
            Authorization: `Bearer ${accessToken}`,
        }

        const url = `/books/${bookId}/ratings`;
        const response = await this.init(headers)
            .put<ApiResult<BookRating>>(url, request);
        if (response.data.status === "error") {
            throw new Error(response.data.message);
        }

        return response.data.data;
    }

    async getRatingsOverview(bookId: number): Promise<GetRatingsOverviewResponse> {
        const url = `/books/${bookId}/ratings/overview`;
        const response = await this.init()
            .get<ApiResult<GetRatingsOverviewResponse>>(url);
        if (response.data.status === "error") {
            throw new Error(response.data.message);
        }

        return response.data.data;
    }
}

export const bookApiHelper = new BookApiHelper();