import {BaseApiHelper} from './BaseApiHelper';
import {AuthenticateUserRequest} from "../models/auth/AuthenticateUserRequest";
import {AuthenticateUserResponse} from "../models/auth/AuthenticateUserResponse";
import {ValidationFailureMessages} from "../models/common/ValidationFailureMessages";
import {ApiResult} from "../models/common/ApiResult";
import {RegisterUserRequest} from "../models/catalog/user/RegisterUserRequest";
import {User} from "../models/catalog/user/User";

class AuthApiHelper extends BaseApiHelper {
    constructor() {
        super();
        this.baseUrl = import.meta.env.VITE_BASE_ADDRESS + "/";
    }

    async authenticate(request: AuthenticateUserRequest): Promise<AuthenticateUserResponse> {
        const response = await this.init()
            .post<ApiResult<AuthenticateUserResponse | ValidationFailureMessages<AuthenticateUserResponse>>>("/auth/authenticate", request);

        if (response.data.status === "error") {
            throw new Error(response.data.message);
        }

        if (response.data.status === "fail") {
            const failureMessages = response.data.data as ValidationFailureMessages<AuthenticateUserResponse>;
            throw new Error(Object.values(failureMessages)[0]);
        }

        return response.data.data as AuthenticateUserResponse;
    }

    async register(request: RegisterUserRequest): Promise<User> {
        const response = await this.init()
            .post<ApiResult<User | ValidationFailureMessages<RegisterUserRequest>>>("/auth/register", request);

        if (response.data.status === "error") {
            throw new Error(response.data.message);
        }

        if (response.data.status === "fail") {
            const failureMessages = response.data.data as ValidationFailureMessages<RegisterUserRequest>;
            throw new Error(Object.values(failureMessages)[0]);
        }

        return response.data.data as User;
    }
}

export const authApiHelper = new AuthApiHelper();