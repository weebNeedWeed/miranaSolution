import {Link} from "react-router-dom";
import {MdOutlineClose} from "react-icons/md";
import {FaTrash} from "react-icons/fa";
import {CurrentlyReading} from "../helpers/models/catalog/currentlyReading/CurrentlyReading";
import {useAccessToken} from "../helpers/hooks/useAccessToken";
import {useQuery} from "react-query";
import {currentlyReadingApiHelper} from "../helpers/apis/CurrentlyReadingApiHelper";
import {bookApiHelper} from "../helpers/apis/BookApiHelper";
import {useSystemContext} from "../contexts/SystemContext";
import {useBaseUrl} from "../helpers/hooks/useBaseUrl";
import {ToastVariant} from "../components/Toast";
import {useLocalStorage} from "../helpers/hooks/useLocalStorage";

type ReadingCardProps = {
    currentlyReading: CurrentlyReading;
    refetch: () => void;
    offline?: boolean;
}

const ReadingCard = ({currentlyReading, refetch, offline}: ReadingCardProps): JSX.Element => {
    const {data: book} = useQuery(
        ["book", currentlyReading.bookId],
        () => bookApiHelper.getBookById(currentlyReading.bookId));

    const _offline = offline ?? false;

    const [accessToken,] = useAccessToken();
    const systemContext = useSystemContext();
    const baseUrl = useBaseUrl();

    if (!book) {
        return <></>
    }

    const handleDelete = async (event: React.MouseEvent<HTMLButtonElement>) => {
        event.preventDefault();

        try {
            await currentlyReadingApiHelper.removeBook(accessToken, book.id);

            systemContext.dispatch({
                type: "addToast",
                payload: {
                    title: "Xoá thành công.",
                    variant: ToastVariant.Success
                }
            });

            await refetch();
        } catch (error: any) {
            systemContext.dispatch({
                type: "addToast",
                payload: {
                    title: "Có lỗi xảy ra, vui lòng thử lại.",
                    variant: ToastVariant.Error
                }
            });
        }
    };

    return (
        <Link
            to={`/books/${book.slug}/chapters/${currentlyReading.chapterIndex}`}
            className="relative group w-full bg-whiteChocolate p-2 flex cursor-pointer rounded shadow-sm shadow-darkVanilla border-2 border-solid hover:border-darkVanilla transition-all mb-3"
        >
            <div className="flex flex-row w-full items-center">
                <img src={`${baseUrl}${book.thumbnailImage}`} alt="" className="aspect-[2/3] w-10"/>
                <div className="flex flex-col h-full justify-start items-start ml-2">
                    <h4 className="text-sm font-semibold text-oldRose line-clamp-1 mt-1">
                        {book.name}
                    </h4>

                    <span className="text-sm text-deepKoamaru mt-1">Số chương: {currentlyReading.chapterIndex}</span>
                </div>
            </div>

            {!_offline && <button
                onClick={handleDelete}
                className="md:hidden md:group-hover:block absolute top-0 right-0 mt-2 mr-2 text-md text-deepKoamaru font-bold"
            >
                <MdOutlineClose/>
            </button>}
        </Link>
    );
};

const CurrentlyReadingsSection = (): JSX.Element => {
    const [accessToken,] = useAccessToken();

    const {data, refetch} = useQuery(
        "currentlyReadings",
        () => currentlyReadingApiHelper.getCurrentlyReadingBooks(accessToken)
    );

    const [offlineReadingBooks,] = useLocalStorage<CurrentlyReading[]>("readings", []);

    return (
        <div className="w-full bg-white rounded p-4 shadow-sm shadow-slate-500 h-full">
            <h2 className="text-xl gradient-text font-bold mb-4">Đang đọc</h2>

            <div className="flex flex-col justify-start items-center">
                {data && data.currentlyReadings.slice(0, 5).map((elm, index) =>
                    <ReadingCard refetch={refetch} currentlyReading={elm} key={index}/>)}

                {!data && offlineReadingBooks.slice(0, 5).map((elm, index) =>
                    <ReadingCard offline={true} refetch={() => {
                    }} currentlyReading={elm} key={index}/>)}
            </div>
        </div>
    );
}

export {CurrentlyReadingsSection};
