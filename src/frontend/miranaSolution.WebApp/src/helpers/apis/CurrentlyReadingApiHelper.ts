import {BaseApiHelper} from "./BaseApiHelper";
import {CurrentlyReading} from "../models/catalog/currentlyReading/CurrentlyReading";
import {AddBookRequest} from "../models/catalog/currentlyReading/AddBookRequest";
import {ApiResult} from "../models/common/ApiResult";
import {Comment} from "../models/catalog/comment/Comment";
import {ValidationFailureMessages} from "../models/common/ValidationFailureMessages";
import {CreateCommentRequest} from "../models/catalog/books/CreateCommentRequest";
import {GetCurrentlyReadingBooksResponse} from "../models/catalog/currentlyReading/getCurrentlyReadingBooksResponse";

class CurrentlyReadingApiHelper extends BaseApiHelper {
    async addBook(accessToken: string, request: AddBookRequest): Promise<void> {
        const headers = {
            Authorization: `Bearer ${accessToken}`,
        }

        const response = await this.init(headers)
            .post<ApiResult<CurrentlyReading>>("/currentlyReadings", request);
        if (response.data.status === "error") {
            throw new Error(response.data.message);
        }
    }

    async removeBook(accessToken: string, bookId: number): Promise<void> {
        const headers = {
            Authorization: `Bearer ${accessToken}`,
        }

        const url = `/currentlyReadings/book/${bookId}`;

        const response = await this.init(headers)
            .delete<ApiResult<void>>(url);
        if (response.data.status === "error") {
            throw new Error(response.data.message);
        }
    }

    async getCurrentlyReadingBooks(accessToken: string, bookId?: number): Promise<GetCurrentlyReadingBooksResponse> {
        const headers = {
            Authorization: `Bearer ${accessToken}`,
        }

        const queryString = new URLSearchParams();
        if (typeof bookId !== "undefined") {
            queryString.append("bookId", bookId!.toString());
        }

        const url = `/currentlyReadings?${queryString.toString()}`;

        const response = await this.init(headers)
            .get<ApiResult<GetCurrentlyReadingBooksResponse>>(url);
        if (response.data.status === "error") {
            throw new Error(response.data.message);
        }

        return response.data.data;
    }
}

export const currentlyReadingApiHelper = new CurrentlyReadingApiHelper();