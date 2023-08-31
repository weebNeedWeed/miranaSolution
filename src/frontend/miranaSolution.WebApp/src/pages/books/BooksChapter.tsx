import {Link, useParams} from "react-router-dom";
import {Book} from "../../helpers/models/catalog/books/Book";
import {Chapter} from "../../helpers/models/catalog/books/Chapter";
import {IoIosArrowBack, IoIosArrowForward} from "react-icons/io";
import {VscBook} from "react-icons/vsc";
import {AiFillEye} from "react-icons/ai";
import {MdOutlineUpdate} from "react-icons/md";
import {BiText} from "react-icons/bi";
import {bookApiHelper} from "../../helpers/apis/BookApiHelper";
import React, {useEffect, useState} from "react";
import {CommentsSection} from "../../containers";
import {RiMenuLine} from "react-icons/ri";
import {FaBookmark, FaHome} from "react-icons/fa";
import {AiOutlineArrowUp} from "react-icons/ai";
import {AnimatePresence, motion} from "framer-motion";
import {MobileChapterList} from "../../containers/MobileChapterList";
import {ToastVariant} from "../../components/Toast";
import {useAuthenticationContext} from "../../contexts/AuthenticationContext";
import {useSystemContext} from "../../contexts/SystemContext";
import {bookmarkApiHelper} from "../../helpers/apis/BookmarkApiHelper";
import {useAccessToken} from "../../helpers/hooks/useAccessToken";
import {Simulate} from "react-dom/test-utils";
import change = Simulate.change;
import {useMediaQuery} from "../../helpers/hooks/useMediaQuery";
import {useLocalStorage} from "../../helpers/hooks/useLocalStorage";
import {userApiHelper} from "../../helpers/apis/UserApiHelper";
import {currentlyReadingApiHelper} from "../../helpers/apis/CurrentlyReadingApiHelper";
import {CurrentlyReading} from "../../helpers/models/catalog/currentlyReading/CurrentlyReading";
import {Helmet} from "react-helmet";
import {Loading} from "../../components";

type ChapterHeaderProps = {
    book: Book;
    chapter: Chapter;
}
const ChapterHeader = ({book, chapter}: ChapterHeaderProps): JSX.Element => {
    const timeFormatOptions: Intl.DateTimeFormatOptions = {
        timeZone: "Asia/Ho_Chi_Minh"
    };

    const updatedAt = new Date(book.updatedAt).toLocaleString("vi-VN", timeFormatOptions);

    return <div className="flex flex-col">
        <div className="flex flex-col items-center">
            <Link to={`/books/${book.slug}`}>
                <h1 className="font-bold text-2xl sm:text-4xl capitalize drop-shadow">{book.name.toLowerCase()}</h1>
            </Link>
            <Link to={"#"}>
                <span className="text-base font-normal drop-shadow">{book.authorName}</span>
            </Link>
            <span
                className="text-base font-normal drop-shadow flex flex-row items-center gap-1"><MdOutlineUpdate/> {updatedAt}</span>
        </div>
        <div
            className="bg-[rgba(255,255,255,0.8)] border-[1px] border-solid border-slate-400 px-2 py-2 sm:px-8 sm:py-4 mt-4">
            <h2 className="font-semibold text-1xl sm:text-2xl">Chương {chapter.index}: {chapter.name}</h2>
            <ul className="list-disc ml-6 text-sm font-normal">
                <li><span className="flex flex-row items-center gap-1"><BiText/> {chapter.wordCount.toString()}</span>
                </li>
                <li><span
                    className="flex flex-row items-center gap-1"><AiFillEye/> {chapter.readCount.toString()}</span>
                </li>
            </ul>
        </div>
    </div>;
};

type PagerSectionProps = {
    chapter: Chapter;
    book: Book;
}
const PagerSection = ({chapter, book}: PagerSectionProps): JSX.Element => {
    const strTemplate = `/books/${book.slug}/chapters/[[chapter]]`;
    const disabledBtnOnClick = (event: any) => {
    };

    return <div className="w-full flex flex-row items-center font-semibold text-base sm:text-xl
        bg-[rgba(255,255,255,0.8)] border-[1px] border-solid border-slate-400 px-2 py-2 sm:px-8 sm:py-4 justify-evenly relative">
        {chapter.previousIndex ?
            <Link className={"flex flex-row items-center grow"}
                  to={strTemplate.replace("[[chapter]]", (chapter.index - 1).toString())}>
                <IoIosArrowBack/> {"Chương trước"}
            </Link> :
            <Link className={"flex flex-row items-center grow text-slate-500 cursor-default"}
                  onClick={e => e.preventDefault()}
                  to={""}>
                <IoIosArrowBack/> {"Chương trước"}
            </Link>}

        <Link to="/" className="border-x-[1px] border-deepKoamaru border-solid px-2 sm:px-6 absolute">
            <VscBook className="text-2xl"/>
        </Link>

        {chapter.nextIndex ?
            <Link className={"flex flex-row items-center grow justify-end"}
                  to={strTemplate.replace("[[chapter]]", (chapter.index + 1).toString())}>
                {"Chương kế"} <IoIosArrowForward/>
            </Link> :
            <Link className={"flex flex-row items-center grow justify-end text-slate-500 cursor-default"}
                  onClick={e => e.preventDefault()}
                  to={""}>
                {"Chương kế"} <IoIosArrowForward/>
            </Link>}
    </div>;
};

const ChapterContent = (props: { book: Book; chapter: Chapter }): JSX.Element => {
    const [openBottom, setOpenBottom] = useState(false);
    const [openDialog, setOpenDialog] = useState(false);
    const authContext = useAuthenticationContext();
    const systemContext = useSystemContext();
    const [accessToken,] = useAccessToken();

    const matches = useMediaQuery("(min-width: 768px)");

    const [readBooks, setReadBooks] = useLocalStorage<string>("readBooks", "");
    const [readChapters, setReadChapters] = useLocalStorage<string>("readChapters", "");

    useEffect(() => {
        const _readBooks = readBooks.split(",").filter(x => x !== "");
        const _readChapters = readChapters.split(",").filter(x => x !== "");
        const bookId = props.book.id.toString();
        const chapterId = props.chapter.id.toString();

        if (authContext.state.isLoggedIn) {
            if (_readBooks.filter(x => x == bookId).length == 0) {
                (async () => {
                    try {
                        await userApiHelper.increaseReadBookCount(accessToken);
                    } catch (error: any) {
                    }
                })();

                _readBooks.push(bookId);
                setReadBooks(_readBooks.join(","));
            }

            if (_readChapters.filter(x => x == chapterId).length == 0) {
                (async () => {
                    try {
                        await userApiHelper.increaseReadChapterCount(accessToken);
                    } catch (error: any) {
                    }
                })();

                _readChapters.push(chapterId);
                setReadChapters(_readChapters.join(","));
            }
        }
    }, []);

    // Handle storing the user's reading books if not logging in
    const [offlineReadingBooks, setOfflineReadingBooks] = useLocalStorage<CurrentlyReading[]>("readings", []);
    useEffect(() => {
        const _books = offlineReadingBooks.filter(x => x.bookId == props.book.id);
        if (_books.length === 0) {
            const newReadingBook: CurrentlyReading = {
                bookId: props.book.id,
                chapterIndex: props.chapter.index,
                userId: "guest",
                createdAt: new Date(Date.now()),
                bookName: props.book.name,
                thumbnailImage: props.book.thumbnailImage,
                bookSlug: props.book.slug,
            };

            const clone = [...offlineReadingBooks];
            clone.push(newReadingBook);

            setOfflineReadingBooks(clone);

            return;
        }

        const _book = _books[0];
        if (_book.chapterIndex !== props.chapter.index) {
            _book.chapterIndex = props.chapter.index;
            _book.createdAt = new Date(Date.now());

            setOfflineReadingBooks([...offlineReadingBooks]);
        }
    }, [props.book.id, props.chapter.index]);

    const variants = {
        initial: {bottom: "-120%"},
        visible: {bottom: 0},
        exit: {bottom: "-120%"}
    }

    const scrollToTop = () => {
        window.scrollTo({top: 0, left: 0, behavior: 'smooth'});
    };

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

        const bookId = props.book.id;
        await bookmarkApiHelper.createBookmark(accessToken, {
            bookId,
            chapterIndex: props.chapter.index
        });

        systemContext.dispatch(
            {
                type: "addToast",
                payload: {
                    variant: ToastVariant.Success,
                    title: `Đánh dấu thành công tại chương ${props.chapter.index}.`
                }
            }
        );
    }

    return <>
        <div className="relative">
            <div onClick={() => setOpenBottom(!openBottom)} className="text-lg tracking-wide sm:text-2xl leading-8 sm:leading-10 bg-[rgba(255,255,255,0.8)] border-[1px]
    border-solid border-slate-400 px-2 py-2 sm:px-8 sm:py-4"
                 dangerouslySetInnerHTML={{__html: props.chapter.content}}>
            </div>

            {matches && <div className="fixed bg-[rgba(255,255,255,0.6)] ml-[-65px] top-32">
                <div className="flex flex-col">
                    <Link
                        to={`/books/${props.book.slug}`}
                        className="flex flex-row w-14 h-14 justify-center items-center text-2xl border-b-[1px] border-deepKoamaru">
                        <FaHome/>
                    </Link>

                    <button
                        onClick={() => setOpenDialog(true)}
                        className="flex flex-row w-14 h-14 justify-center items-center text-2xl border-b-[1px] border-deepKoamaru">
                        <RiMenuLine/>
                    </button>

                    <button
                        onClick={handleCreateBookmark}
                        className="flex flex-row w-14 h-14 justify-center items-center text-2xl border-b-[1px] border-deepKoamaru">
                        <FaBookmark/>
                    </button>

                    <button onClick={scrollToTop}
                            className="w-14 h-14 flex flex-row justify-center items-center text-2xl">
                        <AiOutlineArrowUp/>
                    </button>
                </div>
            </div>}
        </div>


        <AnimatePresence>
            {openBottom && <motion.div
                variants={variants}
                initial="initial"
                animate="visible"
                exit="exit"
                className="md:hidden fixed bottom-0 left-0 w-full bg-oldRose z-30">
                <div className="flex flex-row justify-center items-center py-3 text-deepKoamaru">
                    <Link
                        to={`/books/${props.book.slug}`}
                        className="h-full w-full flex flex-row justify-center items-center text-2xl border-r-[1px] border-deepKoamaru">
                        <FaHome/>
                    </Link>

                    <button
                        onClick={() => setOpenDialog(true)}
                        className="h-full w-full flex flex-row justify-center items-center text-2xl border-r-[1px] border-deepKoamaru">
                        <RiMenuLine/>
                    </button>

                    <button
                        onClick={handleCreateBookmark}
                        className="h-full w-full flex flex-row justify-center items-center text-2xl border-r-[1px] border-deepKoamaru">
                        <FaBookmark/>
                    </button>

                    <button onClick={scrollToTop}
                            className="h-full w-full flex flex-row justify-center items-center text-2xl">
                        <AiOutlineArrowUp/>
                    </button>
                </div>
            </motion.div>}
        </AnimatePresence>

        <MobileChapterList open={openDialog} handleClose={() => setOpenDialog(false)} book={props.book}/>
    </>;
};

const BooksChapter = (): JSX.Element => {
    const {slug, index} = useParams();
    const [chapter, setChapter] = useState<Chapter>();
    const [book, setBook] = useState<Book>();
    const authContext = useAuthenticationContext();
    const [accessToken,] = useAccessToken();

    useEffect(() => {
        (async () => {
            try {
                const result = await bookApiHelper.getBookBySlug(slug!);
                const bookId = result.book.id;

                const _chapter = await bookApiHelper.getBookChapterByIndex(bookId, Number.parseInt(index!, 10));

                setBook(result.book);
                setChapter(_chapter);

                if (authContext.state.isLoggedIn) {
                    await currentlyReadingApiHelper.addBook(accessToken, {bookId, chapterIndex: _chapter.index});
                }
            } catch (error: any) {
            }
        })();
    }, [slug, index]);

    if (!book || !chapter) {
        return <div className="min-h-[700px]">
            <Loading show={true}/>
        </div>
    }

    return <div
        className="mx-auto w-100% md:w-[768px] lg:w-[980px] max-w-full px-8 md:px-0 text-deepKoamaru mb-16">

        <Helmet>
            <title>
                {`${book.name} #${chapter.index}`} | Mirana Readers
            </title>
        </Helmet>

        <ChapterHeader book={book} chapter={chapter}/>

        <div className="my-2">
            <PagerSection chapter={chapter} book={book}/>
        </div>

        <ChapterContent book={book} chapter={chapter}/>

        <div className="my-2">
            <PagerSection chapter={chapter} book={book}/>
        </div>

        <div
            className="my-2 bg-[rgba(255,255,255,0.8)] border-[1px] border-solid border-slate-400 px-2 py-2 sm:px-8 sm:py-4">
            <CommentsSection size={5} bookId={book.id}/>
        </div>
    </div>;
};

export {BooksChapter};