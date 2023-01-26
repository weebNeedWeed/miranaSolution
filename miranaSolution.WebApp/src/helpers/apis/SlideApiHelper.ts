import { BaseApiHelper } from "./BaseApiHelper";
import { Slide } from "./../../types/Slide";

export class SlideApiHelper extends BaseApiHelper {
	readonly GET_ALL = "/slides";

	constructor() {
		super();
	}

	async getAll(): Promise<Array<Slide> | null> {
		try {
			const response = await this.axiosInstance.get(this.GET_ALL);

			return response.data;
		} catch (ex) {
			return null;
		}
	}
}
