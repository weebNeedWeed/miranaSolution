import {User} from "./User";

export interface GetUserProfileRequest {
    user: User;
    totalComments: number;
    totalReactions: number;
    totalBookmarks: number;
    totalUpvotes: number;
    totalRatings: number;
}