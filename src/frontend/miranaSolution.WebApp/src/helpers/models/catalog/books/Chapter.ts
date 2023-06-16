export interface Chapter {
  id: string;
  index: number;
  name: string;
  createdAt: Date;
  updatedAt: Date;
  readCount: number;
  wordCount: number;
  content: string;
}