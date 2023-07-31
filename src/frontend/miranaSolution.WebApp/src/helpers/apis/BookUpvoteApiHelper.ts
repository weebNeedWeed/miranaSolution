import {BaseApiHelper} from "./BaseApiHelper";
import {CreateBookMarkRequest} from "../models/catalog/bookmark/CreateBookMarkRequest";
import {Bookmark} from "../models/catalog/bookmark/Bookmark";
import {ApiResult} from "../models/common/ApiResult";
import {GetAllBookmarksResponse} from "../models/catalog/bookmark/GetAllBookmarksResponse";
import {BookUpvote} from "../models/catalog/books/BookUpvote";
import {GetAllUpvotesResponse} from "../models/catalog/books/GetAllUpvotesResponse";

class BookUpvoteApiHelper extends BaseApiHelper {
    async getAllUpvotes(bookId: number, userId?: string): Promise<BookUpvote[]> {
        let url = `/books/${bookId}/upvotes`;
        if (typeof userId !== "undefined") {
            url += `?userId=${userId}`;
        }

        const response = await this.init()
            .get<ApiResult<GetAllUpvotesResponse>>(url);
        if (response.data.status === "error") {
            throw new Error(response.data.message);
        }

        return response.data.data.bookUpvotes;
    }

    async createUpvote(accessToken: string, bookId: number): Promise<BookUpvote> {
        const headers = {
            Authorization: `Bearer ${accessToken}`,
        }

        const url = `/books/${bookId}/upvotes`;
        const response = await this.init(headers)
            .post<ApiResult<BookUpvote>>(url, {
                bookId: bookId
            });

        if (response.data.status === "error") {
            throw new Error(response.data.message);
        }

        return response.data.data;
    }

    async deleteUpvote(accessToken: string, bookId: number): Promise<void> {
        const headers = {
            Authorization: `Bearer ${accessToken}`,
        }

        const url = `/books/${bookId}/upvotes`;
        const response = await this.init(headers)
            .delete<ApiResult<BookUpvote>>(url);

        if (response.data.status === "error") {
            throw new Error(response.data.message);
        }
    }
}

export const bookUpvoteApiHelper = new BookUpvoteApiHelper();