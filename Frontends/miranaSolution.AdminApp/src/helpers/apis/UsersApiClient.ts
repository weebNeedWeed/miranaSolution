import axios, { AxiosInstance } from "axios";
import { ApiResult } from "../../models/ApiResult";
import { BaseApiClient } from "./BaseApiClient";

type Credentials = {
  userName: string;
  password: string;
}

class UsersApiClient extends BaseApiClient {
  async authenticate(
    credentials: Credentials
  ): Promise<string | null> {
    const response = await this.init().post<ApiResult<{ accessToken: string }>>(
      "/users/authenticate",
      {
        ...credentials,
        rememberMe: false,
      }
    );

    if (response.data.status === "fail" || response.data.status === "error") {
      return null;
    }

    return response.data.data.accessToken;
  }
}

export const usersApiClient = new UsersApiClient();
