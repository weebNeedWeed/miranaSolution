import {Link} from "react-router-dom";
import {Book} from "../helpers/models/catalog/books/Book";
import {useBaseUrl} from "../helpers/hooks/useBaseUrl";
import {FaPen} from "react-icons/fa";

type BookCardProps = {
    book: Book
};
const BookCard = ({book}: BookCardProps): JSX.Element => {
    const baseUrl = useBaseUrl();
    return (
        <div className="w-full md:w-[calc(calc(100%/2)-0.75rem)] md:mr-3 mb-3">
            <Link
                to={`/books/${book.slug}`}
                className="w-full bg-whiteChocolate p-2 flex cursor-pointer rounded shadow-sm shadow-darkVanilla border-2 border-solid hover:border-darkVanilla transition-all"
            >
                <div className="flex flex-row justify-start items-center w-full">
                    <img
                        src={`${baseUrl}${book.thumbnailImage}`}
                        alt=""
                        className="w-14 max-h-[84px] aspect-[2/3] shrink-0"
                    />

                    <div className="flex flex-col ml-2 h-full items-start justify-start">
                        <h4 className="text-sm font-semibold text-oldRose line-clamp-1 uppercase">
                            {book.name}
                        </h4>

                        <p className="text-xs text-deepKoamaru line-clamp-3">
                            {book.shortDescription}
                        </p>

                        <div className="flex flex-row justify-start items-center gap-x-1">
                            <FaPen className="text-oldRose text-xs"/>

                            <span
                                className="text-xs text-oldRose border-b-[1px] border-oldRose font-semibold line-clamp-1">
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