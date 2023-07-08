import axios from "axios";
import {
    AxiosInstance,
    AxiosHeaders,
    RawAxiosRequestHeaders,
    HeadersDefaults,
} from "axios";

export class BaseApiHelper {

    private client: AxiosInstance | null;
    private token: string;
    private baseUrl: string;

    constructor() {
        this.client = null;
        this.baseUrl = "";
        this.token = "";
    }

    init(headers: RawAxiosRequestHeaders = {}): AxiosInstance {
        this.baseUrl = import.meta.env.VITE_BASE_ADDRESS + "api/";

        this.client = axios.create({
            baseURL: this.baseUrl,
            timeout: 1000,
            headers,
        })

        return this.client;
    }
}
