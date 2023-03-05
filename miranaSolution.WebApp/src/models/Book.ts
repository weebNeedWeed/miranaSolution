export interface Book {
  id: string;
  name: string;
  shortDescription: string;
  longDescription: string;
  createdAt: Date;
  updatedAt: Date;
  thumbnailImage: string;
  isRecommended: boolean;
  slug: string;
}