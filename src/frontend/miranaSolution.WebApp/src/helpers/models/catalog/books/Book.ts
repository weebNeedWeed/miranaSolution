export interface Book {
    id: number;
    name: string;
    shortDescription: string;
    longDescription: string;
    createdAt: Date;
    updatedAt: Date;
    thumbnailImage: string;
    isRecommended: boolean;
    slug: string;
    authorName: string;
    authorId: number;
    genres: string[];
    isDone: boolean;
    viewCount: number;
}