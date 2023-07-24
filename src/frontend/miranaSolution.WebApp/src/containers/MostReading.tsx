import {Link} from "react-router-dom";
import {useQuery} from "react-query";
import {bookApiHelper} from "../helpers/apis/BookApiHelper";
import {Book} from "../helpers/models/catalog/books/Book";

type MostReadingCardProps = { book: Book };
const MostReadingCard = (props: MostReadingCardProps): JSX.Element => {
    const {book} = props;

    return (
        <Link
            to={`/books/${book.slug}`}
            className="group w-full sm:w-[calc(calc(100%/2)-20px)] sm:ml-[20px] md:w-[calc(calc(100%/4)-20px)] lg:w-[calc(calc(100%/6)-20px)] bg-whiteChocolate p-2 cursor-pointer rounded shadow-sm shadow-darkVanilla border-2 border-solid hover:border-darkVanilla transition-all mb-[20px]"
        >
            <div className="flex flex-col items-center">
                <img src={book.thumbnailImage} alt="" className="aspect-[2/3] w-full"/>
                <div className="flex flex-col h-full justify-start items-start w-full">
                    <h4 className="text-sm font-semibold text-oldRose line-clamp-1 mt-1">
                        {book.name}
                    </h4>

                    <span className="text-sm text-deepKoamaru mt-1">Lượt đọc: {0}</span>
                </div>
            </div>

            <button
                className="md:hidden md:group-hover:block absolute top-0 right-0 mt-2 mr-2 text-md text-deepKoamaru font-bold"></button>
        </Link>
    );
};

const MostReading = (): JSX.Element => {
    const {isLoading, error, data} = useQuery(
        "mostReading",
        () => bookApiHelper.getMostReadingBooks(20)
    );

    return (
        <div className="w-full bg-white rounded p-4 shadow-sm shadow-slate-500 h-full">
            <h2 className="text-xl gradient-text font-bold">Đọc nhiều</h2>

            <div className="flex flex-row flex-wrap mt-4 sm:ml-[-20px] p-2">
                {data && data.books.map((book, index) =>
                    <MostReadingCard key={index} book={book}/>)}
            </div>
        </div>
    );
};

export {MostReading};
