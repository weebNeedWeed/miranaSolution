import {Chapter} from "./Chapter";

export interface GetAllChaptersResponse {
    chapters: Chapter[];
    pageIndex: number;
    pageSize: number;
    totalPages: number;
    totalChapters: number;
}