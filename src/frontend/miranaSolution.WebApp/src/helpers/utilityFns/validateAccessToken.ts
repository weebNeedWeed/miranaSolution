import jwtDecode, {JwtPayload} from "jwt-decode";
import {userApiHelper} from "../apis/UserApiHelper";
import {GetUserProfileRequest} from "../models/catalog/user/GetUserProfileRequest";

export const getUserByAccessToken = async (accessToken: string): Promise<GetUserProfileRequest | null> => {
    try {
        const decoded = jwtDecode<JwtPayload>(accessToken);
        // Check for expire time
        const exp = decoded.exp!;
        const currentTime = Date.now() / 1000;

        if (exp <= currentTime) {
            return null;
        }

        // Check for user's information
        try {
            const getUserProfileResult = await userApiHelper.getUserProfile(accessToken);
            return getUserProfileResult;
        } catch (error: any) {
            return null;
        }
    } catch (error) {
        return null;
    }
}