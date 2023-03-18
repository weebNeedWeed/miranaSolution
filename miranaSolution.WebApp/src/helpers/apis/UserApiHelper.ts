import { ApiResult } from "../../models/ApiResult";
import { BaseApiHelper } from "./BaseApiHelper";

type AuthenticationData = {
  accessToken: string;
};

class UserApiHelper extends BaseApiHelper {
  async authenticate(credentials: {
    userName: string;
    password: string;
  }): Promise<AuthenticationData | null> {
    var response = await this._axiosInstance.post<
      ApiResult<AuthenticationData>
    >("/users/authenticate", credentials);

    if (response.data.status === "error" || response.data.status === "fail") {
      return null;
    }

    return response.data.data;
  }
}

export const userApiHelper = new UserApiHelper();
