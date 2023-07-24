import {User} from "../catalog/user/User";

export interface AuthenticateUserResponse {
    user: User;
    token: string;
}