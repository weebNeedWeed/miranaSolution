import {ApiResult} from "../models/common/ApiResult";
import {BaseApiHelper} from "./BaseApiHelper";
import {User} from "../models/auth/User";
import {RegisterUserRequest} from "../models/auth/RegisterUserRequest";
import {ValidationFailedMessages} from "../models/common/ValidationFailedMessages";
import {AuthenticateUserRequest} from "../models/auth/AuthenticateUserRequest";
import {AuthenticateUserResponse} from "../models/auth/AuthenticateUserResponse";
import {UpdateUserInformationRequest} from "../models/auth/UpdateUserInformationRequest";
import {UpdateUserPasswordRequest} from "../models/auth/UpdateUserPasswordRequest";

class UserApiHelper extends BaseApiHelper {
    constructor() {
        super();
        this.baseUrl = import.meta.env.VITE_BASE_ADDRESS + "/";
    }

    async authenticate(request: AuthenticateUserRequest): Promise<AuthenticateUserResponse | ValidationFailedMessages<AuthenticateUserResponse>> {
        const response = await this.init()
            .post<ApiResult<AuthenticateUserResponse | ValidationFailedMessages<AuthenticateUserResponse>>>("/auth/authenticate", request);

        if (response.data.status === "error") {
            throw new Error(response.data.message);
        }

        if (response.data.status === "fail") {
            return response.data.data as ValidationFailedMessages<AuthenticateUserResponse>;
        }

        return response.data.data;
    }

    async register(request: RegisterUserRequest): Promise<User | ValidationFailedMessages<RegisterUserRequest>> {
        const response = await this.init()
            .post<ApiResult<User | ValidationFailedMessages<RegisterUserRequest>>>("/auth/register", request);

        if (response.data.status === "error") {
            throw new Error(response.data.message);
        }

        if (response.data.status === "fail") {
            return response.data.data as ValidationFailedMessages<RegisterUserRequest>;
        }

        return response.data.data as User;
    }

    async getUserInformation(accessToken: string): Promise<User> {
        const headers = {
            Authorization: `Bearer ${accessToken}`,
        }

        const response = await this.init(headers).get<ApiResult<User>>("/auth/information");
        if (response.data.status === "error") {
            throw new Error(response.data.message);
        }

        return response.data.data;
    }

    async updateUserInformation(accessToken: string, request: UpdateUserInformationRequest): Promise<User | ValidationFailedMessages<UpdateUserInformationRequest>> {
        const headers = {
            Authorization: `Bearer ${accessToken}`,
        }

        const formData = new FormData();
        if (typeof request.avatar !== "undefined") {
            formData.append("avatar", request.avatar);
        }

        formData.append("firstName", request.firstName);
        formData.append("lastName", request.lastName);
        formData.append("email", request.email);

        const response = await this.init(headers).post<ApiResult<
            User | ValidationFailedMessages<UpdateUserInformationRequest>>>("/auth/information", formData);
        if (response.data.status === "error") {
            throw new Error(response.data.message);
        }

        if (response.data.status === "fail") {
            return response.data.data as ValidationFailedMessages<UpdateUserInformationRequest>;
        }

        return response.data.data;
    }

    async updateUserPassword(accessToken: string, request: UpdateUserPasswordRequest): Promise<User | ValidationFailedMessages<UpdateUserPasswordRequest>> {
        const headers = {
            Authorization: `Bearer ${accessToken}`,
        }

        const response = await this.init(headers)
            .post<ApiResult<User | ValidationFailedMessages<UpdateUserPasswordRequest>>>("/auth/password", request);
        if (response.data.status === "error") {
            throw new Error(response.data.message);
        }

        if (response.data.status === "fail") {
            return response.data.data as ValidationFailedMessages<UpdateUserPasswordRequest>;
        }

        return response.data.data;
    }
}

export const userApiHelper = new UserApiHelper();
