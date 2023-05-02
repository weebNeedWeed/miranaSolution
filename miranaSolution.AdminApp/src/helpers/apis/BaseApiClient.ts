import axios from "axios";
import { AxiosInstance, RawAxiosRequestHeaders} from "axios";

export class BaseApiClient {
  private baseUrl: string;
  private client: AxiosInstance | null;

  constructor() {
    this.client = null;
    this.baseUrl = import.meta.env.VITE_BASE_ADDRESS;
  }

  init(headers: RawAxiosRequestHeaders = {}) {
    this.client = axios.create({
      baseURL: this.baseUrl,
      timeout: 1000,
      headers: headers
    });

    return this.client;
  }
}