import {BookCard, Divider, Section} from "../../components";
import {FaSwatchbook} from "react-icons/fa";
import {AiOutlinePlus, AiFillCheckCircle, AiOutlineClose} from "react-icons/ai";
import {CgTimelapse} from "react-icons/cg";
import {useQuery} from "react-query";
import {genreApiHelper} from "../../helpers/apis/GenreApiHelper";
import {useNavigate, useLocation, useSearchParams} from "react-router-dom";
import clsx from "clsx";
import {bookApiHelper} from "../../helpers/apis/BookApiHelper";
import {BookGetPagingRequest} from "../../helpers/models/catalog/books/BookGetPagingRequest";
import {useEffect, useMemo, useState} from "react";
import {Genre} from "../../helpers/models/catalog/books/Genre";
import {useMediaQuery} from "../../helpers/hooks/useMediaQuery";
import {IoMdArrowDropdown} from "react-icons/io";

type FilterButtonProps = {
    title: string;
    onClick: (event: React.MouseEvent<HTMLButtonElement>) => void;
    isActive?: boolean;
}
const FilterButton = (props: FilterButtonProps): JSX.Element => {
    const {title, onClick, isActive} = props;
    const activeClass = "bg-deepKoamaru text-white";

    return <button
        onClick={onClick}
        className={clsx("text-sm rounded-lg border-deepKoamaru border-solid border-[1px] p-1 flex flex-row items-center", isActive && activeClass)}>
        {isActive ? <AiOutlineClose/> : <AiOutlinePlus/>}
        <p>{title}</p>
    </button>
}

type FilterSectionInnerProps = {
    genres: Array<Genre>,
    isGenreWithIdActive: (id: number) => boolean,
    handleFilterByGenreFnFactory: (id: number) => (event: React.MouseEvent<HTMLButtonElement>) => void
};
const FilterSectionInner = (props: FilterSectionInnerProps) => {
    const {genres, isGenreWithIdActive, handleFilterByGenreFnFactory} = props;
    const isMobile = useMediaQuery("(max-width: 760px)");
    const [openFilterDialog, setOpenFilterDialog] = useState<boolean>(false);

    useEffect(() => {
        if (openFilterDialog) {
            document.body.style.overflowY = "hidden";
            return () => {
                document.body.style.overflowY = "scroll";
            };
        }
    }, [openFilterDialog]);

    if (isMobile) {
        return <div className="px-4 py-2 w-full bg-darkVanilla">
            <button onClick={() => setOpenFilterDialog(!openFilterDialog)}
                    className="flex flex-row justify-center items-center w-full cursor-pointer gap-x-1 bg-deepKoamaru rounded text-white px-3 py-1.5">
                <AiOutlinePlus className="text-lg"/>
                <span className="font-medium text-base">Bộ lọc truyện</span>
            </button>

            {openFilterDialog &&
                <div className="fixed top-0 left-0 w-full h-full block bg-darkVanilla z-20">
                    <div className="flex flex-col justify-start items-center">
                        <div
                            className="w-full text-center py-2.5 bg-deepKoamaru text-white text-xl capitalize relative shadow-deepKoamaru shadow-md">
                            <h4>Bộ lọc truyện</h4>
                            <button onClick={() => setOpenFilterDialog(!openFilterDialog)}
                                    className="absolute top-0 right-3 h-full flex flex-row items-center cursor-pointer">
                                <AiOutlineClose/>
                            </button>
                        </div>
                        <div className="bg-transparent min-h-full w-full p-3">
                            <div>
                                <span className="flex flex-row justify-start items-center gap-x-2">
                                    <AiFillCheckCircle/>
                                    <p className="text-lg font-semibold">Đã chọn</p>
                                </span>

                                <div className="flex flex-row flex-wrap mt-2 gap-2">
                                </div>
                            </div>

                            <hr className="border-deepKoamaru w-3/4 mx-auto my-5"/>

                            <div>
                                <span className="flex flex-row justify-start items-center gap-x-2">
                                    <FaSwatchbook/>
                                    <p className="text-lg font-semibold">Thể loại</p>
                                </span>

                                <div className="flex flex-row flex-wrap mt-2 gap-2">
                                    {genres.map(genre =>
                                        <FilterButton isActive={isGenreWithIdActive(genre.id)} key={genre.id}
                                                      title={genre.name}
                                                      onClick={handleFilterByGenreFnFactory(genre.id)}/>)}
                                </div>
                            </div>

                            <hr className="border-deepKoamaru w-3/4 mx-auto my-5"/>

                            <div>
                                <span className="flex flex-row justify-start items-center gap-x-2">
                                    <CgTimelapse/>
                                    <p className="text-lg font-semibold">Trạng thái</p>
                                </span>

                                <div className="flex flex-row flex-wrap mt-2 gap-2">
                                    <button
                                        className="text-sm rounded-lg border-deepKoamaru border-solid border-[1px] p-1 flex flex-row items-center">
                                        <AiOutlinePlus/>
                                        <p>Chưa hoàn thành</p>
                                    </button>

                                    <button
                                        className="text-sm rounded-lg border-deepKoamaru border-solid border-[1px] p-1 flex flex-row items-center">
                                        <AiOutlinePlus/>
                                        <p>Đã hoàn thành</p>
                                    </button>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>}
        </div>;
    }

    return <div className="hidden w-[25%] md:flex flex-col shrink-0 grow-0 bg-darkVanilla justify-start p-4">

        <div>
          <span className="flex flex-row justify-start items-center gap-x-2">
            <AiFillCheckCircle/>
            <p className="text-lg font-semibold">Đã chọn</p>
          </span>

            <div className="flex flex-row flex-wrap mt-2 gap-2">
            </div>
        </div>

        <hr className="border-deepKoamaru w-3/4 mx-auto my-6"/>

        <div>
            <span className="flex flex-row justify-start items-center gap-x-2">
            <FaSwatchbook/>
            <p className="text-lg font-semibold">Thể loại</p>
            </span>

            <div className="flex flex-row flex-wrap mt-2 gap-2">
                {genres.map(genre =>
                    <FilterButton isActive={isGenreWithIdActive(genre.id)} key={genre.id} title={genre.name}
                                  onClick={handleFilterByGenreFnFactory(genre.id)}/>)}
            </div>
        </div>

        <hr className="border-deepKoamaru w-3/4 mx-auto my-6"/>

        <div>
          <span className="flex flex-row justify-start items-center gap-x-2">
            <CgTimelapse/>
            <p className="text-lg font-semibold">Trạng thái</p>
          </span>

            <div className="flex flex-row flex-wrap mt-2 gap-2">
                <button
                    className="text-sm rounded-lg border-deepKoamaru border-solid border-[1px] p-1 flex flex-row items-center">
                    <AiOutlinePlus/>
                    <p>Chưa hoàn thành</p>
                </button>

                <button
                    className="text-sm rounded-lg border-deepKoamaru border-solid border-[1px] p-1 flex flex-row items-center">
                    <AiOutlinePlus/>
                    <p>Đã hoàn thành</p>
                </button>
            </div>
        </div>
    </div>;
}

type FIltersSectionProps = {};
const FilterSection = (props: FIltersSectionProps): JSX.Element => {
    const navigate = useNavigate();
    const location = useLocation();
    const [searchParams] = useSearchParams();
    const {isLoading, error, data: genresData} = useQuery(
        "genres",
        () => genreApiHelper.getAll(),
        {
            staleTime: Infinity
        });
    if (isLoading || error || !genresData) {
        return <div></div>;
    }

    const isGenreWithIdActive = (id: number): boolean => {
        let genres: string | null = searchParams.get("genres");
        return genres !== null && genres.split(",").findIndex(x => x == id.toString()) > -1;
    }
    const handleFilterByGenreFnFactory = (id: number) => {
        return (event: React.MouseEvent<HTMLButtonElement>) => {
            let genres: string | null = searchParams.get("genres");

            // Handle remove genre if selected
            if (isGenreWithIdActive(id)) {
                const temp = genres!.split(",").filter(x => x !== id.toString() && x !== "");
                genres = temp.join(",");

            } else if (!genres) {
                genres = id.toString();
            } else {
                const temp = genres.split(",").filter(x => x !== "");
                temp.push(id.toString());
                genres = temp.join(",");
            }

            let queryString = "";
            for (let entry of searchParams.entries()) {
                if (entry[0] === "genres") {
                    continue;
                }

                queryString += `${entry[0]}=${entry[1]}&`;
            }

            queryString += `genres=${genres}`;
            navigate(location.pathname + `?${queryString}`);
        }
    }

    return <FilterSectionInner genres={genresData}
                               isGenreWithIdActive={isGenreWithIdActive}
                               handleFilterByGenreFnFactory={handleFilterByGenreFnFactory}/>;
}

type SearchFormProps = {};
const SearchForm = (props: SearchFormProps): JSX.Element => {
    return <form
        className="bg-darkVanilla w-full flex flex-row justify-start items-center px-4 pb-4 pt-0 md:pt-4 gap-4">
        <input type="text"
               className="border-b-2 border-deepKoamaru outline-none bg-transparent grow h-full py-2 pl-4"/>
        <button type="button" className="px-2 py-1 bg-deepKoamaru text-white rounded">Tìm</button>
    </form>
}

const getRequest = (
    _pageIndex: number,
    _pageSize: number,
    _genreIds: string | null): BookGetPagingRequest => {

    const _request: BookGetPagingRequest = {
        pageSize: _pageSize,
        pageIndex: _pageIndex
    };

    if (Boolean(_genreIds) && _genreIds !== "") {
        _request.genreIds = _genreIds!;
    }

    return _request;
}

type BookCardListProps = {};
const BookCardList = (props: BookCardListProps): JSX.Element => {
    const [searchParams] = useSearchParams();

    const pageSize = searchParams.get("pageSize") != null ? parseInt(searchParams.get("pageSize")!, 10) : 15;
    const pageIndex = searchParams.get("pageIndex") != null ? parseInt(searchParams.get("pageIndex")!, 10) : 1;
    const genreIds = searchParams.get("genres");

    let request = useMemo<BookGetPagingRequest>(
        () => getRequest(pageIndex, pageSize, genreIds), [searchParams]
    )

    const {isLoading, error, data} = useQuery(["books", request.pageIndex, request.pageSize, request.genreIds],
        () => bookApiHelper.getPaging(request),
        {staleTime: Infinity});

    return <div className="w-full p-5 bg-[rgba(255,255,255,0.8)] h-full">
        <div className="flex flex-row flex-wrap md:mr-[-0.75rem]">
            {(isLoading || error || !data) && <>Loading...</>}
            {data && data.items.map(book => <BookCard key={book.id} name={book.name}
                                                      shortDescription={book.shortDescription}
                                                      thumbnailImage={book.thumbnailImage}/>)}
        </div>
    </div>
}

const BooksIndex = (): JSX.Element => {
    return <Section className="text-deepKoamaru pt-2">
        <div className="flex flex-col md:flex-row items-stretch shadow-sm shadow-slate-500">
            <FilterSection/>

            <div className="grow">
                <div className="flex flex-col h-full">
                    <SearchForm/>

                    <BookCardList/>
                </div>
            </div>
        </div>
    </Section>;
};

export {BooksIndex};