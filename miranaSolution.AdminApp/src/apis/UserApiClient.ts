import axios, { AxiosInstance } from "axios";
import { ApiResult } from "../models/ApiResult";

class UserApiClient {
  axiosInstance: AxiosInstance;

  constructor() {
    this.axiosInstance = axios.create({
      baseURL: import.meta.env.VITE_BASE_ADDRESS,
    });
  }

  async authenticate(
    userName: string,
    password: string
  ): Promise<string | null> {
    const response = await this.axiosInstance.post<ApiResult<string>>(
      "/users/authenticate",
      {
        userName,
        password,
        rememberMe: false,
      }
    );

    if (response.data.status === "fail" || response.data.status === "error") {
      return null;
    }

    return response.data.data;
  }
}

export const userApiClient = new UserApiClient();
