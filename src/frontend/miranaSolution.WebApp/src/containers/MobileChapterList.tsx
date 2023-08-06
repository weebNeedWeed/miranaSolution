import React, {useEffect} from "react";
import {AiOutlineClose} from "react-icons/ai";
import clsx from "clsx";
import {Link, useSearchParams} from "react-router-dom";
import {useQuery} from "react-query";
import {bookApiHelper} from "../helpers/apis/BookApiHelper";
import {Pager} from "../components";
import {Book} from "../helpers/models/catalog/books/Book";
import {useMediaQuery} from "../helpers/hooks/useMediaQuery";

type MobileChapterListProps = {
    open: boolean;
    handleClose: () => void;
    book: Book;
}

const MobileChapterList = (props: MobileChapterListProps): JSX.Element => {
    const {open, handleClose, book} = props;
    const [searchParams] = useSearchParams();

    const pageIndexKey = "chapterKey";
    const pageSizeKey = "chapterSize";

    const pageIndex = parseInt(searchParams.get(pageIndexKey) ?? "1", 10);
    const pageSize = parseInt(searchParams.get(pageSizeKey) ?? "30", 10);

    const matches = useMediaQuery("(min-width: 768px)");

    useEffect(() => {
        if (open) {
            document.body.style.overflowY = "hidden";

            return () => {
                document.body.style.overflowY = "scroll";
            }
        }
    }, [open]);

    const {data: getChaptersResponse} = useQuery(
        ["chapters", book.id, pageIndex, pageSize],
        () => bookApiHelper.getAllChapters({
            bookId: book.id,
            pageIndex,
            pageSize
        })
    );

    if (!getChaptersResponse) {
        return <></>
    }

    if (matches) {
        const renderArray: JSX.Element[][] = [];
        const sizeOfRow = 3;
        getChaptersResponse.chapters.forEach((chapter, index) => {
            if (index % sizeOfRow === 0) {
                renderArray.push([]);
            }
            const actualIndex = index / sizeOfRow;
            renderArray[actualIndex].push(
                <Link
                    key={index}
                    to={`/books/${book.slug}/chapters/${chapter.index}`}
                    className="w-1/3 pr-6 line-clamp-1">Chương {chapter.index}: {chapter.name}
                </Link>);
        });

        return <div
            className={clsx(!open && "hidden", "fixed top-0 left-0 w-full h-full bg-whiteChocolate z-40 overflow-y-auto")}>
            <div className="w-full h-full flex flex-col">
                <div
                    className="w-full text-center py-2.5 bg-deepKoamaru text-white text-lg relative shadow-deepKoamaru shadow-md">
                    <h4>Danh sách chương</h4>
                    <button onClick={handleClose}
                            className="absolute top-0 right-3 h-full flex flex-row items-center cursor-pointer">
                        <AiOutlineClose/>
                    </button>
                </div>

                <ul className="flex flex-col p-4">
                    {renderArray.map((sub, index) =>
                        <li key={index}
                            className="py-3 flex flex-row border-b-[1px] border-[rgba(48,54,89,0.4)]">
                            {sub}
                        </li>)}
                </ul>

                <div className="mt-auto">
                    <Pager pageIndex={getChaptersResponse.pageIndex} pageSize={getChaptersResponse.pageSize}
                           totalPages={getChaptersResponse.totalPages}/>
                </div>
            </div>
        </div>
    }

    return <div
        className={clsx(!open && "hidden", "fixed top-0 left-0 w-full h-full bg-whiteChocolate z-40 overflow-y-auto")}>
        <div className="w-full h-full flex flex-col">
            <div
                className="w-full text-center py-2.5 bg-deepKoamaru text-white text-lg relative shadow-deepKoamaru shadow-md">
                <h4>Danh sách chương</h4>
                <button onClick={handleClose}
                        className="absolute top-0 right-3 h-full flex flex-row items-center cursor-pointer">
                    <AiOutlineClose/>
                </button>
            </div>

            <div className="bg-transparent w-full mt-2 flex flex-col">
                {getChaptersResponse.chapters.map(chapter =>
                    <Link key={chapter.id}
                          onClick={handleClose}
                          to={`/books/${book.slug}/chapters/${chapter.index}`}
                          className="w-full px-3 border-b-[1px] border-deepKoamaru text-base py-2">
                        Chương {chapter.index}: {chapter.name}
                    </Link>)}
            </div>

            <div className="mt-auto">
                <Pager pageIndex={getChaptersResponse.pageIndex} pageSize={getChaptersResponse.pageSize}
                       totalPages={getChaptersResponse.totalPages}/>
            </div>
        </div>
    </div>
}

export {MobileChapterList};