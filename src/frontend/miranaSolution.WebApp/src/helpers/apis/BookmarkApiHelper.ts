import {BaseApiHelper} from "./BaseApiHelper";
import {CreateBookMarkRequest} from "../models/catalog/bookmark/CreateBookMarkRequest";
import {Bookmark} from "../models/catalog/bookmark/Bookmark";
import {ApiResult} from "../models/common/ApiResult";
import {GetAllBookmarksResponse} from "../models/catalog/bookmark/GetAllBookmarksResponse";

class BookmarkApiHelper extends BaseApiHelper {
    async createBookmark(accessToken: string, request: CreateBookMarkRequest): Promise<Bookmark> {
        const headers = {
            Authorization: `Bearer ${accessToken}`,
        }

        const response = await this.init(headers)
            .post<ApiResult<Bookmark>>("/bookmarks", request);

        if (response.data.status === "error") {
            throw new Error(response.data.message);
        }

        return response.data.data;
    }

    async getAllBookmark(accessToken: string, bookId?: number): Promise<Bookmark[]> {
        const headers = {
            Authorization: `Bearer ${accessToken}`,
        }
        const url = "/bookmarks" + (bookId && `?bookId=${bookId}`);
        const response = await this.init(headers)
            .get<ApiResult<GetAllBookmarksResponse>>(url);

        if (response.data.status === "error") {
            throw new Error(response.data.message);
        }

        return response.data.data.bookmarks;
    }

    async deleteBookmark(accessToken: string, bookId: number): Promise<void> {
        const headers = {
            Authorization: `Bearer ${accessToken}`,
        }
        const url = `/bookmarks/book/${bookId}`;
        const response = await this.init(headers)
            .delete<ApiResult<void>>(url);

        if (response.data.status === "error") {
            throw new Error(response.data.message);
        }
    }
}

export const bookmarkApiHelper = new BookmarkApiHelper();