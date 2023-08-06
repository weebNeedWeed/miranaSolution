import {Link} from "react-router-dom";
import {Book} from "../helpers/models/catalog/books/Book";

type BookCardProps = {
    book: Book
};
const BookCard = ({book}: BookCardProps): JSX.Element => {
    return (
        <div className="md:w-[calc(calc(100%/2)-0.75rem)] md:mr-3 mb-3">
            <Link
                to={`/books/${book.slug}`}
                className="w-full bg-whiteChocolate p-2 flex cursor-pointer rounded shadow-sm shadow-darkVanilla border-2 border-solid hover:border-darkVanilla transition-all"
            >
                <div className="flex flex-row justify-center items-center">
                    <img
                        src={book.thumbnailImage}
                        alt=""
                        className="w-14 max-h-[84px] aspect-[2/3]"
                    />

                    <div className="flex flex-col ml-2 h-full items-start justify-start">
                        <h4 className="text-sm font-semibold text-oldRose line-clamp-1 uppercase">
                            {book.name}
                        </h4>

                        <p className="text-xs text-deepKoamaru line-clamp-3">
                            {book.shortDescription}
                        </p>

                        <div className="flex flex-row justify-start items-center">
                            <span
                                className="text-xs text-oldRose border-b-[1px] border-oldRose font-semibold">
                                {book.authorName}
                            </span>
                        </div>
                    </div>
                </div>
            </Link>
        </div>
    );
};

export {BookCard};