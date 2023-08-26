import {FaTrash} from "react-icons/fa";
import {CurrentlyReading} from "../../helpers/models/catalog/currentlyReading/CurrentlyReading";
import {useQuery} from "react-query";
import {bookApiHelper} from "../../helpers/apis/BookApiHelper";
import {Link} from "react-router-dom";
import {currentlyReadingApiHelper} from "../../helpers/apis/CurrentlyReadingApiHelper";
import {useAccessToken} from "../../helpers/hooks/useAccessToken";
import {useBaseUrl} from "../../helpers/hooks/useBaseUrl";
import {useSystemContext} from "../../contexts/SystemContext";
import {ToastVariant} from "../../components/Toast";
import {Helmet} from "react-helmet";
import React from "react";

type CurrentlyReadingBlockProps = {
    currentlyReading: CurrentlyReading;
    refetch: () => void
};
const CurrentlyReadingBlock = ({currentlyReading, refetch}: CurrentlyReadingBlockProps): JSX.Element => {
    const {data: book} = useQuery(
        ["book", currentlyReading.bookId],
        () => bookApiHelper.getBookById(currentlyReading.bookId));

    const [accessToken,] = useAccessToken();
    const systemContext = useSystemContext();
    const baseUrl = useBaseUrl();

    if (!book) {
        return <></>
    }

    const handleDelete = async () => {
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
    }

    return <div className="flex flex-row w-full md:w-[calc(50%-0.5rem)] gap-x-2.5 shadow-md rounded flex-wrap shrink-0">
        <div className="aspect-[3/4] w-12 h-auto"
             style={{backgroundImage: `url("${baseUrl}${book.thumbnailImage}")`}}>
        </div>
        <div className="flex flex-col grow">
            <Link to={`/books/${book.slug}/chapters/${currentlyReading.chapterIndex}`}
                  className="w-full line-clamp-1 text-oldRose font-semibold">
                {book.name}
            </Link>
            <span className="text-sm">
                Số chương: {currentlyReading.chapterIndex}
            </span>
            <div className="flex flex-row justify-end mr-2">
                <button onClick={handleDelete} className="text-xs text-red-500">
                    <FaTrash/>
                </button>
            </div>
        </div>
    </div>;
};

const UserCurrentlyReadings = (): JSX.Element => {
    const [accessToken,] = useAccessToken();

    const {data, refetch} = useQuery(
        "currentlyReadings",
        () => currentlyReadingApiHelper.getCurrentlyReadingBooks(accessToken)
    );

    if (!data) {
        return <></>
    }

    return <div className="w-full">
        <Helmet>
            <title>Currently Readings | Mirana Readers</title>
        </Helmet>

        <div className="w-full flex flex-row flex-wrap gap-2 md:gap-4">
            {data.currentlyReadings.map((elm, index) =>
                <CurrentlyReadingBlock refetch={refetch} currentlyReading={elm} key={index}/>)}
        </div>
    </div>
}
export {UserCurrentlyReadings};