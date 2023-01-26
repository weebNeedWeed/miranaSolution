import axios from "axios";
import {
	AxiosInstance,
	AxiosHeaders,
	RawAxiosRequestHeaders,
	HeadersDefaults,
} from "axios/index";

export class BaseApiHelper {
	axiosInstance: AxiosInstance;

	constructor(headers: RawAxiosRequestHeaders = {}) {
		this.axiosInstance = axios.create({
			baseURL: import.meta.env.VITE_BASE_ADDRESS,
			timeout: 1000,
			headers,
		});
	}
}
