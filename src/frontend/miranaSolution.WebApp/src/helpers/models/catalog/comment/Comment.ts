export interface Comment {
    id: number,
    content: string,
    parentId?: number,
    createdAt: Date,
    updatedAt: Date,
    userId: string,
    username: string,
    userAvatar: string,
    bookId: number
}