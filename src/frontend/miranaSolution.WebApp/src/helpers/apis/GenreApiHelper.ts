import {BaseApiHelper} from "./BaseApiHelper";
import {Genre} from "../models/catalog/books/Genre";
import {ApiResult} from "../models/common/ApiResult";

class GenreApiHelper extends BaseApiHelper {
    async getAll(): Promise<Array<Genre>> {
        const response = await this.init().get<ApiResult<Array<Genre>>>("/genres");
        return response.data.data;
    }
}

export const genreApiHelper = new GenreApiHelper();