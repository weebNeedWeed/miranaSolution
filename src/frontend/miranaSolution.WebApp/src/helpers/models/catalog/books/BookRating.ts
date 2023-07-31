export interface BookRating {
    userId: string;
    bookId: number;
    content: string;
    star: number;
    createdAt: Date;
    updatedAt: Date;
}