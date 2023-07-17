export interface UpdateUserInformationRequest {
    firstName: string;
    lastName: string;
    email: string;
    avatar?: File;
}