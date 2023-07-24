import {BaseApiHelper} from "./BaseApiHelper";
import {Genre} from "../models/catalog/books/Genre";
import {ApiResult} from "../models/common/ApiResult";
import {GetAllGenresResponse} from "../models/catalog/genre/GetAllGenresResponse";

class GenreApiHelper extends BaseApiHelper {
    async getAllGenres(): Promise<Array<Genre>> {
        const response = await this.init().get<ApiResult<GetAllGenresResponse>>("/genres");

        if (response.data.status === "error") {
            throw new Error(response.data.message);
        }

        return response.data.data.genres;
    }
}

export const genreApiHelper = new GenreApiHelper();