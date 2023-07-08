export interface UserUpdateInfoRequest {
    firstName: string;
    lastName: string;
    email: string;
    avatar?: File;
}