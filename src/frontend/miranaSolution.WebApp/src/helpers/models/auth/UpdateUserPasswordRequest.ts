export interface UpdateUserPasswordRequest {
    currentPassword: string;
    newPassword: string;
    newPasswordConfirmation: string;
}