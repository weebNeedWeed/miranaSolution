import { LoaderFunctionArgs } from "react-router-dom";
import { bookApiHelper } from "../../apis/BookApiHelper";
import { Chapter } from "../../models/catalog/books/Chapter";

type BooksInfoLoaderArgs = {
  params: {
    slug: string;
    index: number;
  }
}

export async function booksInfoLoader(args: LoaderFunctionArgs): Promise<Chapter | null> {
  const { slug, index } = args.params;
  const book = await bookApiHelper.getBookBySlug(slug!);
  if (!book) {
    return null;
  }

  const chapter = await bookApiHelper.getChapterByIndex(book!.id, Number.parseInt(index!, 10));
  if (!chapter) {
    return null;
  }


  return chapter;
}