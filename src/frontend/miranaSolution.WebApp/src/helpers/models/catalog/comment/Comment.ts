export interface Comment {
    id: number,
    content: string,
    parentId?: number,
    createdAt: Date,
    updatedAt: Date,
    userId: string,
    bookId: number
}