import {ApiResult} from "../models/ApiResult";
import {BaseApiHelper} from "./BaseApiHelper";
import {UserRegisterRequest} from "../models/auth/UserRegisterRequest";
import {User} from "../models/auth/User";

type AuthenticationData = {
  accessToken: string;
};

export type ValidateFailedMessage<T> = {
  [P in keyof T]: string[];
}

class UserApiHelper extends BaseApiHelper {
  async authenticate(credentials: {
    userName: string;
    password: string;
  }): Promise<AuthenticationData | null> {
    const response = await this.init().post<
      ApiResult<AuthenticationData>
    >("/users/authenticate", credentials);

    if (response.data.status === "error" || response.data.status === "fail") {
      return null;
    }

    return response.data.data;
  }

  async register(userRegisterRequest: UserRegisterRequest): Promise<User | ValidateFailedMessage<UserRegisterRequest> | null> {
    const response = await this.init()
      .post<ApiResult<User | ValidateFailedMessage<UserRegisterRequest>>>("/users", userRegisterRequest);

    if (response.data.status === "error") {
      return null;
    }

    if (response.data.status === "fail") {
      return response.data.data as ValidateFailedMessage<UserRegisterRequest>;
    }

    return response.data.data as User;
  }
}

export const userApiHelper = new UserApiHelper();