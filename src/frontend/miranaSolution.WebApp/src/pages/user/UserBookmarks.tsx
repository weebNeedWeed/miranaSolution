import {Link} from "react-router-dom";
import {FaTrash} from "react-icons/fa";
import {Bookmark} from "../../helpers/models/catalog/bookmark/Bookmark";
import {useQuery} from "react-query";
import {bookApiHelper} from "../../helpers/apis/BookApiHelper";
import {useAccessToken} from "../../helpers/hooks/useAccessToken";
import {useSystemContext} from "../../contexts/SystemContext";
import {useBaseUrl} from "../../helpers/hooks/useBaseUrl";
import {currentlyReadingApiHelper} from "../../helpers/apis/CurrentlyReadingApiHelper";
import {ToastVariant} from "../../components/Toast";
import {bookmarkApiHelper} from "../../helpers/apis/BookmarkApiHelper";

type BookmarkBlockProps = {
    bookmark: Bookmark;
    refetch: () => void;
}
const BookmarkBlock = ({bookmark, refetch}: BookmarkBlockProps): JSX.Element => {
    const {data: book} = useQuery(
        ["book", bookmark.bookId],
        () => bookApiHelper.getBookById(bookmark.bookId));

    const [accessToken,] = useAccessToken();
    const systemContext = useSystemContext();
    const baseUrl = useBaseUrl();

    if (!book) {
        return <></>
    }

    const handleDelete = async () => {
        try {
            await bookmarkApiHelper.deleteBookmark(accessToken, book.id);

            systemContext.dispatch({
                type: "addToast",
                payload: {
                    title: "Huỷ đánh dấu thành công.",
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
            <Link to={`/books/${book.slug}/chapters/${bookmark.chapterIndex}`}
                  className="w-full line-clamp-1 text-oldRose font-semibold">
                {book.name}
            </Link>
            <span className="text-sm">
                Chương: {bookmark.chapterIndex}
            </span>
            <div className="flex flex-row justify-end mr-2">
                <button onClick={handleDelete} className="text-xs text-red-500">
                    <FaTrash/>
                </button>
            </div>
        </div>
    </div>;
}


const UserBookmarks = (): JSX.Element => {
    const [accessToken,] = useAccessToken();

    const {data, refetch} = useQuery(
        "bookmarks",
        () => bookmarkApiHelper.getAllBookmark(accessToken)
    );

    if (!data) {
        return <></>
    }

    return <div className="flex w-full flex-row flex-wrap gap-2 md:gap-4">
        {data.map((elm, index) =>
            <BookmarkBlock refetch={refetch} bookmark={elm} key={index}/>)}
    </div>
}

export {UserBookmarks};