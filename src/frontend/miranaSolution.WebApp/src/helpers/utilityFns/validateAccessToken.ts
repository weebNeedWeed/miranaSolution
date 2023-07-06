import jwtDecode, {JwtPayload} from "jwt-decode";
import {userApiHelper} from "../apis/UserApiHelper";
import {User} from "../models/auth/User";

export const getUserByAccessToken = async (accessToken: string): Promise<User | null> => {
    try {
        const decoded = jwtDecode<JwtPayload>(accessToken);
        const issuer = "mirana.com";

        // Check for issuer and audience
        if (decoded.aud !== decoded.iss || decoded.aud !== issuer) {
            return null;
        }

        // Check for expire time
        const exp = decoded.exp!;
        const currentTime = Date.now() / 1000;

        if (exp <= currentTime) {
            return null;
        }

        // Check for user's information
        const user = await userApiHelper.getUserInformation(accessToken);
        if (user === null) {
            return null;
        }

        return user;
    } catch (error) {
        return null;
    }
}