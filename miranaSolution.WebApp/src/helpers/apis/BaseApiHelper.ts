import axios from "axios";
import {
	AxiosInstance,
	AxiosHeaders,
	RawAxiosRequestHeaders,
	HeadersDefaults,
} from "axios/index";

export class BaseApiHelper {
	_axiosInstance: AxiosInstance;

	get axiosInstance() {
		return this._axiosInstance;
	}

	constructor(headers: RawAxiosRequestHeaders = {}) {
		this._axiosInstance = axios.create({
			baseURL: import.meta.env.VITE_BASE_ADDRESS,
			timeout: 1000,
			headers,
		});
	}
}
