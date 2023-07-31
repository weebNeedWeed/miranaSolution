import axios, {AxiosInstance, RawAxiosRequestHeaders} from "axios";

export class BaseApiHelper {

    public client: AxiosInstance | null;
    public baseUrl: string;

    constructor() {
        this.client = null;
        this.baseUrl = import.meta.env.VITE_BASE_ADDRESS + "api/";
    }

    init(headers: RawAxiosRequestHeaders = {}): AxiosInstance {
        this.client = axios.create({
            baseURL: this.baseUrl,
            timeout: 1000,
            headers,
        })

        return this.client;
    }
}
