import {Link, useLoaderData} from "react-router-dom";
import {Book} from "../../helpers/models/catalog/books/Book";
import {Chapter} from "../../helpers/models/catalog/books/Chapter";
import {IoIosArrowBack, IoIosArrowForward} from "react-icons/io";
import {VscBook} from "react-icons/vsc";
import {AiFillEye} from "react-icons/ai";
import {MdOutlineUpdate} from "react-icons/md";
import {BiText} from "react-icons/bi";

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
        {chapter.hasPreviousChapter ?
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

        {chapter.hasNextChapter ?
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

const ChapterContent = (props: { content: string }): JSX.Element => {
    return <div className="text-xl tracking-wide sm:text-2xl leading-10 bg-[rgba(255,255,255,0.8)] border-[1px]
        border-solid border-slate-400 px-2 py-2 sm:px-8 sm:py-4"
                dangerouslySetInnerHTML={{__html: props.content}}></div>;
};

const BooksChapter = (): JSX.Element => {
    const loaderData: any = useLoaderData();

    if (!loaderData) {
        return <div>error</div>;
    }

    const book = loaderData.book as Book;
    const chapter = loaderData.chapter as Chapter;

    return <div
        className="mx-auto w-100% md:w-[768px] lg:w-[980px] max-w-full px-8 md:px-0 text-deepKoamaru mb-16">
        <ChapterHeader book={book} chapter={chapter}/>

        <div className="my-2">
            <PagerSection chapter={chapter} book={book}/>
        </div>

        <ChapterContent content={chapter.content}/>

        <div className="my-2">
            <PagerSection chapter={chapter} book={book}/>
        </div>
    </div>;
};

export {BooksChapter};