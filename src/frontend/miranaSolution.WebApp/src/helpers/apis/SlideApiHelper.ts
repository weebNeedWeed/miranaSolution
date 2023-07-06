import {BaseApiHelper} from "./BaseApiHelper";
import {Slide} from "../models/catalog/sildes/Slide";
import {ApiResult} from "../models/ApiResult";

class SlideApiHelper extends BaseApiHelper {
    async getAll(): Promise<Array<Slide> | null> {
        try {
            const response = await this.init().get<ApiResult<Array<Slide>>>("/slides");
            return response.data.data;
        } catch (ex) {
            return null;
        }
    }
}

export const slideApiHelper = new SlideApiHelper;