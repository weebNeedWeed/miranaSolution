import {Comment} from "../comment/Comment";

export interface GetAllCommentsResponse {
    comments: Comment[];
    pageIndex: number;
    pageSize: number;
    totalComments: number;
    totalPages: number;
}