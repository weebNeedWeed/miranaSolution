import {BaseApiHelper} from './BaseApiHelper';
import {ApiResult} from "../models/common/ApiResult";
import {CountCommentReactionResponse} from "../models/catalog/comment/CountCommentReactionResponse";

class CommentApiHelper extends BaseApiHelper {
    async createCommentReaction(accessToken: string, commentId: number): Promise<void> {
        const headers = {
            Authorization: `Bearer ${accessToken}`,
        }

        const url = `/comments/${commentId}/reaction`;
        const response = await this.init(headers)
            .post<ApiResult<void>>(url);
        if (response.data.status === "error") {
            throw new Error(response.data.message);
        }
    }

    async countCommentReaction(commentId: number): Promise<CountCommentReactionResponse> {
        const url = `/comments/${commentId}/reaction/counting`;
        const response = await this.init()
            .get<ApiResult<CountCommentReactionResponse>>(url);
        if (response.data.status === "error") {
            throw new Error(response.data.message);
        }

        return response.data.data;
    }

    async checkUserIsReacted(accessToken: string, commentId: number): Promise<boolean> {
        const headers = {
            Authorization: `Bearer ${accessToken}`,
        }

        const url = `/comments/${commentId}/reaction`;
        const response = await this.init(headers)
            .get<ApiResult<{ isReacted: boolean }>>(url);
        if (response.data.status === "error") {
            throw new Error(response.data.message);
        }

        return response.data.data.isReacted;
    }

    async deleteCommentReaction(accessToken: string, commentId: number): Promise<void> {
        const headers = {
            Authorization: `Bearer ${accessToken}`,
        }

        const url = `/comments/${commentId}/reaction`;
        const response = await this.init(headers)
            .delete<ApiResult<void>>(url);
        if (response.data.status === "error") {
            throw new Error(response.data.message);
        }
    }
}

export const commentApiHelper = new CommentApiHelper();