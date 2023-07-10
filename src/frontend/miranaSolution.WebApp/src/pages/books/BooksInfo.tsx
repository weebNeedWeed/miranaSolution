import {Section} from "../../components";
import {Link, useNavigate, useParams, useSearchParams} from "react-router-dom";
import {useEffect, useState} from "react";
import {bookApiHelper} from "../../helpers/apis/BookApiHelper";
import {Book} from "../../helpers/models/catalog/books/Book";
import clsx from "clsx";

const TabsSection = (props: { book: Book }): JSX.Element => {
    const [tabIndex, setTabIndex] = useState<1 | 2>(1);

    const isTabIndexActive = (index: number) => {
        return index === tabIndex;
    }

    const getActiveTabButtonClassName = (tabIndex: number) => {
        return clsx(isTabIndexActive(tabIndex) ? "border-deepKoamaru" : "border-[rgba(48,54,89,0.4)] text-[rgba(48,54,89,0.4)]", "px-6 py-2 text-lg border-b-2 font-semibold border-solid");
    }

    return <div className="flex flex-col w-full">
        <div className="w-full flex flex-row">
            <button onClick={() => setTabIndex(1)}
                    className={getActiveTabButtonClassName(1)}>
                Thông tin chi tiết
            </button>

            <button onClick={() => setTabIndex(2)}
                    className={getActiveTabButtonClassName(2)}>
                Danh sách chương (88 chương)
            </button>
        </div>

        <div className="w-full min-h-[300px]">
            <div className={clsx(isTabIndexActive(1) ? "block" : "hidden", "w-full")}>
                {props.book.longDescription}
            </div>

            <div className={clsx(isTabIndexActive(2) ? "block" : "hidden", "w-full")}>
                <ul className="flex flex-col px-2 mt-4">
                    {Array.from(new Array(30)).map((_, index) => <li key={index}
                                                                     className="py-3 flex flex-row border-b-[1px] border-[rgba(48,54,89,0.4)]">
                        <Link
                            to={"/"}
                            className="w-1/3 pr-6 line-clamp-1">Chương {index}: Người ở rể thiếu niên, nhìn thấy thu
                            quang</Link>
                        <Link
                            to={"/"}
                            className="w-1/3 pr-6 line-clamp-1">Chương {index}: Nha hoàn Thanh Nguyệt, người vô tuyệt
                            cảnh</Link>
                        <Link
                            to={"/"} className="w-1/3 pr-6 line-clamp-1">Chương {index}: Nam gia tiểu thư tới</Link>
                    </li>)}
                </ul>
            </div>
        </div>
    </div>;
}

const BooksInfo = (): JSX.Element => {
    const {slug} = useParams<string>();
    const navigate = useNavigate();
    const [book, setBook] = useState<Book>({} as Book);

    useEffect(() => {
        (async () => {
            const book = await bookApiHelper.getBookBySlug(slug!);
            if (book === null) {
                // TODO: Navigate the user to not found page
                navigate("/404");
                return;
            }

            setBook(book);
        })();
    }, [slug]);

    return <Section className="text-deepKoamaru">
        <div className="w-full bg-darkVanilla shadow-sm shadow-slate-500 flex flex-col">
            <div className="w-full flex flex-row items-stretch p-6">
                <span
                    className="w-36 aspect-[3/4] shrink-0 block rounded-md bg-cover bg-center bg-no-repeat drop-shadow-md"
                    style={{backgroundImage: `url('${book?.thumbnailImage}')`}}>
                </span>

                <div className="ml-4 grow w-full flex flex-col">
                    <span className="text-3xl font-bold capitalize">{book.name}</span>

                    <div className="flex flex-row mt-2 gap-x-2">
                        <span className="text-xs py-1 px-3 rounded-3xl bg-deepKoamaru text-white">
                            Tag1
                        </span>
                        <span className="text-xs py-1 px-3 rounded-3xl bg-deepKoamaru text-white">
                            Tag2
                        </span>
                    </div>

                    <div className="mt-auto flex flex-row">
                        <Link
                            to={`/books/${book.slug}/chapters/1`}
                            className="item rounded bg-deepKoamaru py-2 px-3 mt-4 text-white flex justify-center items-center gap-x-2 text-sm md:text-base">
                            Đọc truyện
                        </Link>
                    </div>
                </div>
            </div>

            <div className="w-full mt-6 flex flex-col">
                <TabsSection book={book}/>
            </div>
        </div>
    </Section>;
};

export {BooksInfo};