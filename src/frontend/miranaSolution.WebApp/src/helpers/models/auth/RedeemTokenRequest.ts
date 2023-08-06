export interface RedeemTokenRequest {
    email: string;
    token: string;
    newPassword: string;
    newPasswordConfirmation: string;
}