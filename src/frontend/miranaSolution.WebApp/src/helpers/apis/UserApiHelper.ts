import {ApiResult} from "../models/common/ApiResult";
import {BaseApiHelper} from "./BaseApiHelper";
import {User} from "../models/catalog/user/User";
import {ValidationFailureMessages} from "../models/common/ValidationFailureMessages";
import {UpdateUserProfileRequest} from "../models/catalog/user/UpdateUserProfileRequest";
import {UpdateUserPasswordRequest} from "../models/catalog/user/UpdateUserPasswordRequest";
import {GetUserProfileRequest} from "../models/catalog/user/GetUserProfileRequest";

class UserApiHelper extends BaseApiHelper {
    async getUserProfile(accessToken: string): Promise<GetUserProfileRequest> {
        const headers = {
            Authorization: `Bearer ${accessToken}`,
        }

        const response = await this.init(headers).get<ApiResult<GetUserProfileRequest>>("/users/profile");
        if (response.data.status === "error") {
            throw new Error(response.data.message);
        }

        return response.data.data;
    }

    async updateUserProfile(accessToken: string, request: UpdateUserProfileRequest): Promise<User> {
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
            User | ValidationFailureMessages<UpdateUserProfileRequest>>>("/users/profile", formData);
        if (response.data.status === "error") {
            throw new Error(response.data.message);
        }

        if (response.data.status === "fail") {
            const failureMessages = response.data.data as ValidationFailureMessages<UpdateUserProfileRequest>;
            throw new Error(Object.values(failureMessages)[0]);
        }

        return response.data.data as User;
    }

    async updateUserPassword(accessToken: string, request: UpdateUserPasswordRequest): Promise<User> {
        const headers = {
            Authorization: `Bearer ${accessToken}`,
        }

        const response = await this.init(headers)
            .post<ApiResult<User | ValidationFailureMessages<UpdateUserPasswordRequest>>>("/users/password", request);
        if (response.data.status === "error") {
            throw new Error(response.data.message);
        }

        if (response.data.status === "fail") {
            const failureMessages = response.data.data as ValidationFailureMessages<UpdateUserPasswordRequest>;
            throw new Error(Object.values(failureMessages)[0]);
        }

        return response.data.data as User;
    }
}

export const userApiHelper = new UserApiHelper();
