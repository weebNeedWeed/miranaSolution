import {BaseApiHelper} from "./BaseApiHelper";
import {Slide} from "../models/catalog/sildes/Slide";
import {ApiResult} from "../models/common/ApiResult";

class SlideApiHelper extends BaseApiHelper {
    async getAll(): Promise<Array<Slide>> {
        const response = await this.init().get<ApiResult<Array<Slide>>>("/slides");

        if (response.data.status === "error") {
            throw new Error(response.data.message);
        }

        return response.data.data;
    }
}

export const slideApiHelper = new SlideApiHelper;