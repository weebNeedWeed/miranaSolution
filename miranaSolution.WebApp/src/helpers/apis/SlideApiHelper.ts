import { BaseApiHelper } from "./BaseApiHelper";
import { Slide } from "./../../types/Slide";
import { ApiResult } from "../../types/ApiResult";

class SlideApiHelper extends BaseApiHelper {
	readonly GET_ALL = "/slides";

	constructor() {
		super();
	}

	async getAll(): Promise<Array<Slide> | null> {
		try {
			const response = await this.axiosInstance.get<ApiResult<Array<Slide>>>(this.GET_ALL);

			if(response.data.isSucceed === false) {
				throw new Error();
			}

			return response.data.data;
		} catch (ex) {
			return null;
		}
	}
}

export const slideApiHelper = new SlideApiHelper;