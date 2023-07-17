import {LoaderFunctionArgs} from "react-router-dom";
import {bookApiHelper} from "../../apis/BookApiHelper";
import {Chapter} from "../../models/catalog/books/Chapter";
import {Book} from "../../models/catalog/books/Book";

type BooksInfoLoaderArgs = {
    params: {
        slug: string;
        index: number;
    }
}

export async function booksInfoLoader(args: LoaderFunctionArgs): Promise<{ chapter: Chapter; book: Book } | null> {
    const {slug, index} = args.params;
    const book = await bookApiHelper.getBookBySlug(slug!);
    if (!book) {
        return null;
    }

    const chapter = await bookApiHelper.getBookChapterByIndex(book!.id, Number.parseInt(index!, 10));
    if (!chapter) {
        return null;
    }


    return {chapter, book};
}