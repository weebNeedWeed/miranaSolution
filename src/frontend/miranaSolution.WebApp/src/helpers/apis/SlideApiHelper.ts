import {BaseApiHelper} from "./BaseApiHelper";
import {Slide} from "../models/catalog/sildes/Slide";
import {ApiResult} from "../models/common/ApiResult";
import {GetAllSlidesResponse} from "../models/catalog/sildes/GetAllSlidesResponse";

class SlideApiHelper extends BaseApiHelper {
    async getAllSlides(): Promise<GetAllSlidesResponse> {
        const response = await this.init().get<ApiResult<GetAllSlidesResponse>>("/slides");
        
        if (response.data.status === "error") {
            throw new Error(response.data.message);
        }

        return response.data.data;
    }
}

export const slideApiHelper = new SlideApiHelper;