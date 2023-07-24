import {Avatar, Pager, Rating, Section} from "../../components";
import {Link, useNavigate, useParams, useSearchParams} from "react-router-dom";
import {useEffect, useState} from "react";
import {bookApiHelper} from "../../helpers/apis/BookApiHelper";
import {Book} from "../../helpers/models/catalog/books/Book";
import clsx from "clsx";
import {GetAllChaptersResponse} from "../../helpers/models/catalog/books/GetAllChaptersResponse";
import {AiFillLike, AiFillRead, AiFillStar} from "react-icons/ai";
import {BsFillBookmarkCheckFill, BsFillBookmarkFill, BsFillReplyFill} from "react-icons/bs";
import {BiTime, BiUpArrowAlt} from "react-icons/bi";
import {GetBookBySlugRequest} from "../../helpers/models/catalog/books/GetBookBySlugRequest";
import authorIcon from "./../../assets/author.jpg";
import {timeSince} from "../../helpers/utilityFns/timeSince";
import {IoMdArrowDropdown} from "react-icons/io";
import {Bookmark} from "../../helpers/models/catalog/bookmark/Bookmark";
import {bookmarkApiHelper} from "../../helpers/apis/BookmarkApiHelper";
import {useAccessToken} from "../../helpers/hooks/useAccessToken";
import {useSystemContext} from "../../contexts/SystemContext";
import {useAuthenticationContext} from "../../contexts/AuthenticationContext";
import {ToastVariant} from "../../components/Toast";

type ChapterListProps = {
    [T in keyof GetAllChaptersResponse]: GetAllChaptersResponse[T]
}
const ChapterList = (props: ChapterListProps): JSX.Element => {
    const {pageSize, pageIndex, totalPages, chapters} = props;

    const {slug} = useParams<string>();

    const renderArray: JSX.Element[][] = [];
    const sizeOfRow = 3;
    chapters.forEach((chapter, index) => {
        if (index % sizeOfRow === 0) {
            renderArray.push([]);
        }
        const actualIndex = index / sizeOfRow;
        renderArray[actualIndex].push(
            <Link
                key={index}
                to={`/books/${slug}/chapters/${chapter.index}`}
                className="w-1/3 pr-6 line-clamp-1">Chương {chapter.index}: {chapter.name}
            </Link>);
    });

    return <>
        <ul className="flex flex-col">
            {renderArray.map((sub, index) =>
                <li key={index}
                    className="py-3 flex flex-row border-b-[1px] border-[rgba(48,54,89,0.4)]">
                    {sub}
                </li>)}
        </ul>

        <div className="mt-auto">
            <Pager pageIndex={pageIndex} pageSize={pageSize} totalPages={totalPages}/>
        </div>
    </>
}

type TabsSectionProps = {
    getChaptersResponse: GetAllChaptersResponse;
    book: Book,
};
const TabsSection = ({getChaptersResponse, book}: TabsSectionProps): JSX.Element => {
    const [tabIndex, setTabIndex] = useState<1 | 2>(1);

    const isTabIndexActive = (index: number) => {
        return index === tabIndex;
    }

    const getActiveTabButtonClassName = (tabIndex: number) => {
        return clsx(isTabIndexActive(tabIndex) ? "border-deepKoamaru" : "text-[rgba(48,54,89,0.4)]", "px-6 py-2 text-lg border-b-[1px] font-semibold border-solid");
    }

    return <div className="flex flex-col w-full">
        <div className="w-full flex flex-row border-[rgba(48,54,89,0.4)] border-b-[1px] box-border">
            <button onClick={() => setTabIndex(1)}
                    className={getActiveTabButtonClassName(1)}>
                Thông tin chi tiết
            </button>

            <button onClick={() => setTabIndex(2)}
                    className={getActiveTabButtonClassName(2)}>
                Danh sách chương ({getChaptersResponse.totalChapters} chương)
            </button>
        </div>

        <div className="w-full min-h-[650px] px-8 py-8 flex flex-col h-full">
            <div className={clsx(isTabIndexActive(1) ? "block" : "hidden", "w-full")}>
                <div className="flex flex-row w-full">
                    <div className="mr-4 w-full flex flex-col">
                        {book.longDescription}
                    </div>
                    <div
                        className="flex flex-col bg-oldRose rounded-md py-6 min-w-[300px] min-h-[500px] h-full justify-start items-start">
                        <div className="flex flex-row w-full justify-center">
                            <Avatar className="w-24 h-24" imageUrl={authorIcon}/>
                        </div>
                        <div
                            className="text-center w-full text-base font-semibold mt-3 border-[rgba(0,0,0,0.2)] border-solid border-b-[1px]">{book.authorName}</div>

                        <span className="text-center w-full text-base font-semibold mt-4">Cùng tác giả</span>

                        <div
                            className="flex flex-col w-full justify-center items-center text-base font-normal mt-1">
                            <Link
                                to={"/"}
                                className="w-full text-center line-clamp-1 border-b-[1px] border-[rgba(0,0,0,0.2)] border-solid mb-2">
                                A
                            </Link>
                            <Link
                                to={"/"}
                                className="w-full text-center line-clamp-1 border-b-[1px] border-[rgba(0,0,0,0.2)] border-solid mb-2">
                                A
                            </Link>
                        </div>
                    </div>
                </div>
            </div>

            <div className={clsx(isTabIndexActive(2) ? "flex" : "hidden", "flex-col w-full h-full grow")}>
                <ChapterList chapters={getChaptersResponse.chapters}
                             pageIndex={getChaptersResponse.pageIndex}
                             pageSize={getChaptersResponse.pageSize}
                             totalPages={getChaptersResponse.totalPages}
                             totalChapters={getChaptersResponse.totalChapters}/>
            </div>
        </div>
    </div>;
}

const BooksInfo = (): JSX.Element => {
    const {slug} = useParams<string>();
    const navigate = useNavigate();
    const [getBookResponse, setBookResponse] = useState<GetBookBySlugRequest>();
    const [getAllChaptersResponse, setResponse] = useState<GetAllChaptersResponse>();
    const [bookmark, setBookmark] = useState<Bookmark>();
    const [searchParams] = useSearchParams();
    const [accessToken, setAccessToken] = useAccessToken();
    const systemContext = useSystemContext();
    const authContext = useAuthenticationContext();

    const pageIndex = parseInt(searchParams.get("pageIndex") ?? "1", 10);
    const pageSize = parseInt(searchParams.get("pageSize") ?? "30", 10);

    useEffect(() => {
        (async () => {
            try {
                const result = await bookApiHelper.getBookBySlug(slug!);
                setBookResponse(result);

                const chaptersResult = await bookApiHelper.getAllChapters({
                    bookId: result.book.id,
                    pageIndex,
                    pageSize
                });

                setResponse(chaptersResult);

                if (authContext.state.isLoggedIn) {
                    const bookmarks = await bookmarkApiHelper.getAllBookmark(accessToken, result.book.id);
                    setBookmark(bookmarks[0]);
                }
            } catch (error: any) {
                // TODO: Navigate the user to not found page
                navigate("/404");
            }
        })();
    }, [slug, pageIndex, pageSize]);

    if (!getBookResponse) {
        return <></>;
    }

    const book = getBookResponse.book;

    const handleCreateBookmark = async () => {
        if (!authContext.state.isLoggedIn) {
            systemContext.dispatch(
                {
                    type: "addToast",
                    payload: {
                        variant: ToastVariant.Warning,
                        title: "Vui lòng đăng nhập để đánh dấu."
                    }
                }
            );
            return;
        }

        const bookId = book.id;
        if (bookmark) {
            await bookmarkApiHelper.deleteBookmark(accessToken, bookId);
            systemContext.dispatch(
                {
                    type: "addToast",
                    payload: {
                        variant: ToastVariant.Success,
                        title: "Huỷ đánh dấu thành công."
                    }
                }
            );
            setBookmark(undefined);
            return;
        }

        const _bookmark = await bookmarkApiHelper.createBookmark(accessToken, {
            bookId,
            chapterIndex: 1
        });
        systemContext.dispatch(
            {
                type: "addToast",
                payload: {
                    variant: ToastVariant.Success,
                    title: "Đánh dấu thành công."
                }
            }
        );
        setBookmark(_bookmark);
    }

    return <Section className="text-deepKoamaru">
        <div
            className="w-full bg-[rgba(255,255,255,0.8)] rounded-md shadow-sm shadow-slate-500 flex flex-col">
            <div className="w-full flex flex-row items-stretch p-6">
                <span
                    className="w-40 aspect-[3/4] shrink-0 block rounded-md bg-cover bg-center bg-no-repeat drop-shadow-md"
                    style={{backgroundImage: `url('${book.thumbnailImage}')`}}>
                </span>

                <div className="ml-4 grow w-full flex flex-col">
                    <span className="text-2xl font-extrabold capitalize">{book.name}</span>

                    <div className="flex flex-row mt-2 gap-x-2">
                        <span className="text-xs py-1 px-3 rounded-3xl bg-oldRose text-white">
                            {book.authorName}
                        </span>

                        <span className="text-xs py-1 px-3 rounded-3xl bg-darkVanilla text-white">
                            {book.isDone ? "Hoàn thành" : "Chưa hoàn thành"}
                        </span>

                        {book.genres.map((genre, index) => <span
                            className="text-xs py-1 px-3 rounded-3xl bg-darkVanilla text-white">
                            {genre}
                        </span>)}
                    </div>

                    <div className="flex flex-row justify-start items-center mt-2 gap-x-8">
                        <div className="flex flex-col items-start">
                            <span className="font-semibold text-xl">
                                {getAllChaptersResponse?.totalPages}
                            </span>

                            <span className="font-normal text-base">
                                Chương
                            </span>
                        </div>

                        <div className="flex flex-col items-start">
                            <span className="font-semibold text-xl">
                                0
                            </span>

                            <span className="font-normal text-base">
                                Lượt đọc
                            </span>
                        </div>

                        <div className="flex flex-col items-start">
                            <span className="font-semibold text-xl">
                                0
                            </span>

                            <span className="font-normal text-base">
                                Cất giữ
                            </span>
                        </div>

                        <div className="flex flex-col items-start">
                            <span className="font-semibold text-xl">
                                {getBookResponse.totalUpvotes}
                            </span>

                            <span className="font-normal text-base">
                                Đề cử
                            </span>
                        </div>
                    </div>

                    <div className="mt-2 w-full flex flex-row gap-x-1">
                        <Rating value={5}/>
                        <span className="text-base text-deepKoamaru">
                            <span className="font-semibold">4</span>
                            <span>/5 (5 đánh giá)</span>
                        </span>
                    </div>

                    <div className="mt-auto flex flex-row gap-x-4">
                        <Link
                            to={`/books/${book.slug}/chapters/1`}
                            className="item rounded bg-deepKoamaru py-1.5 px-3 mt-4 text-white font-semibold flex justify-center items-center gap-x-2 text-sm">
                            <AiFillRead/> Đọc truyện
                        </Link>

                        <button
                            className="item rounded bg-transparent py-1.5 px-3 mt-4 text-deepKoamaru font-semibold flex justify-center items-center gap-x-2 text-sm border-deepKoamaru border-2"
                        >
                            <AiFillStar/> Đánh giá
                        </button>

                        {!bookmark ? <button
                            onClick={handleCreateBookmark}
                            className="item rounded bg-transparent py-1.5 px-3 mt-4 text-deepKoamaru font-semibold flex justify-center items-center gap-x-2 text-sm border-deepKoamaru border-2"
                        >
                            <BsFillBookmarkFill/> Đánh dấu
                        </button> : <button
                            onClick={handleCreateBookmark}
                            className="item rounded bg-deepKoamaru py-1.5 px-3 mt-4 text-white font-semibold flex justify-center items-center gap-x-2 text-sm"
                        >
                            <BsFillBookmarkCheckFill/> Đã đánh dấu
                        </button>}


                        <button
                            className="item rounded bg-transparent py-1.5 px-3 mt-4 text-deepKoamaru font-semibold flex justify-center items-center gap-x-2 text-sm border-deepKoamaru border-2"
                        >
                            <BiUpArrowAlt/> Đề cử
                        </button>
                    </div>
                </div>
            </div>

            <div className="w-full mt-6 flex flex-col">
                {getAllChaptersResponse && <TabsSection book={book} getChaptersResponse={getAllChaptersResponse}/>}
            </div>

            <div className="text-deepKoamaru p-6">
                <div className="font-semibold text-xl">
                    Bình luận (122)
                </div>
                <form className="flex flex-row w-full gap-x-4 mt-3 flex-wrap">
                    <Avatar className="w-12 h-12"/>
                    <textarea
                        className="rounded-2xl grow resize-none px-3 py-1 border-oldRose border-2 border-solid outline-none"></textarea>

                    <div className="w-full flex justify-end pt-2">
                        <button className="text-base rounded-xl px-2 py-1 bg-oldRose">
                            Bình luận
                        </button>
                    </div>
                </form>

                <div className="flex flex-col w-full mt-3">
                    <div className="w-full flex flex-row gap-x-2 border-t-[2px] pt-2">
                        <Avatar className="w-12 h-12 shrink-0"/>
                        <div className="flex flex-col">
                            <span className="font-semibold text-base">Meomeo</span>
                            <span className="flex flex-row items-center gap-x-1 font-normal text-xs">
                                <BiTime/>
                                {timeSince(new Date(Date.now()))}
                            </span>

                            <span className="mt-2">
                                đoạn đầu là nhân vật chính trả thù + tác giả tận lực miêu tả con người biến chất trong tận thế để cho truyện dark một xíu thôi, về sau từ lúc hắn đổi căn cứ thì quay trở lại chính tuyến là phát triển thế lực, khai thác dị năng, v.v. đạo hữu thấy đoạn đầu cấn quá thì nhảy sang chương 170 trở đi mà xem
                            </span>

                            <span className="mt-2 flex flex-row justify-between items-center font-semibold">
                                <button className="flex flex-row items-center">
                                    Xem 2 câu trả lời
                                    <IoMdArrowDropdown className="text-xl"/>
                                </button>
                                <div className="flex flex-row gap-x-6">
                                    <button
                                        className="flex flex-row items-center gap-x-0.5 text-oldRose">
                                        <AiFillLike className="text-base"/>
                                        0
                                    </button>

                                    <button className="flex flex-row items-center">
                                        <BsFillReplyFill className="text-lg"/>
                                        Trả lời
                                    </button>
                                </div>
                            </span>

                            <div className="w-full flex flex-row gap-x-2 border-t-[2px] pt-2 mt-2">
                                <Avatar className="w-12 h-12 shrink-0"/>
                                <div className="flex flex-col">
                                    <span className="font-semibold text-base">Meomeo</span>
                                    <span className="mt-0.5">
                                                Thiếu chương rồi ad ơi. 2414-2417 . Nhảy cóc quá
                                            </span>

                                    <div className="flex flex-row gap-x-6">
                                        <button
                                            className="flex flex-row items-center gap-x-0.5 text-oldRose">
                                            <AiFillLike className="text-base"/>
                                            0
                                        </button>

                                        <span
                                            className="flex flex-row items-center gap-x-1 font-normal text-xs">
                                                    <BiTime/>
                                            {timeSince(new Date(Date.now()))}
                                                </span>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </Section>;
};

export {BooksInfo};