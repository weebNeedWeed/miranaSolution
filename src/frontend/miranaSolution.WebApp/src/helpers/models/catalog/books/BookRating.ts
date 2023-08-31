export interface BookRating {
    userId: string;
    username: string;
    userAvatar: string;
    bookId: number;
    content: string;
    star: number;
    createdAt: Date;
    updatedAt: Date;
}