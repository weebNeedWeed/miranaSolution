export interface UserUpdatePasswordRequest {
    oldPassword: string;
    newPassword: string;
    newPasswordConfirmation: string;
}