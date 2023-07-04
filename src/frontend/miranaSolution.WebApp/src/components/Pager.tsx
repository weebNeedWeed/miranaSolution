import {VscArrowSmallLeft, VscArrowSmallRight} from "react-icons/vsc";
import {PagedResult} from "../helpers/models/common/PagedResult";
import {useLocation, useNavigate, useSearchParams} from "react-router-dom";
import clsx from "clsx";

type PagerButtonProps = {
    children: React.ReactNode;
    onClick?: (event: React.MouseEvent<HTMLButtonElement>) => void,
    isActive?: boolean
};
const PagerButton = (props: PagerButtonProps): JSX.Element => {
    const {children, onClick, isActive} = props;

    return <button onClick={!isActive ? onClick : () => {
    }}
                   className={clsx(isActive ? "border-deepKoamaru" : "border-oldRose", "border-2 w-8 h-8 bg-oldRose rounded flex justify-center items-center border-solid")}>
        {children}
    </button>;
}

type PagerProps = {
    pagedResult: PagedResult<any>
};
const Pager = (props: PagerProps): JSX.Element => {
    const {pagedResult} = props;
    const [searchParams] = useSearchParams();
    const location = useLocation();
    const navigate = useNavigate();

    const doesPreviousButtonExist = pagedResult.pageIndex !== 1;
    const doesNextButtonExist = pagedResult.pageIndex !== pagedResult.totalPages;

    const handleGoToFirstPage = (event: React.MouseEvent<HTMLButtonElement>) => {
        const queryString = new URLSearchParams();

        for (let entry of searchParams.entries()) {
            if (entry[0] === "pageIndex") {
                continue;
            }

            queryString.append(entry[0], entry[1]);
        }

        if (doesPreviousButtonExist) {
            queryString.append("pageIndex", "1");
        }

        navigate(location.pathname + "?" + queryString.toString());
    }

    const handleGoToLastPage = (event: React.MouseEvent<HTMLButtonElement>) => {
        const queryString = new URLSearchParams();

        for (let entry of searchParams.entries()) {
            if (entry[0] === "pageIndex") {
                continue;
            }

            queryString.append(entry[0], entry[1]);
        }

        if (doesPreviousButtonExist) {
            queryString.append("pageIndex", pagedResult.totalPages.toString());
        }

        navigate(location.pathname + "?" + queryString.toString());
    }

    const handleGoToSpecificPageFnFactory = (pageNumber: number) =>
        (event: React.MouseEvent<HTMLButtonElement>) => {
            const queryString = new URLSearchParams();

            for (let entry of searchParams.entries()) {
                if (entry[0] === "pageIndex") {
                    continue;
                }

                queryString.append(entry[0], entry[1]);
            }

            if (doesPreviousButtonExist) {
                queryString.append("pageIndex", pageNumber.toString());
            }

            navigate(location.pathname + "?" + queryString.toString());
        }

    const startPageNumber = (pagedResult.pageIndex - 1) < 1 ? 1 : pagedResult.pageIndex - 1;
    const endPageNumber = (pagedResult.pageIndex + 1) > pagedResult.totalPages ? pagedResult.totalPages : pagedResult.pageIndex + 1;

    const numberButtons: JSX.Element[] = [];
    for (let page = startPageNumber; page <= endPageNumber; ++page) {
        numberButtons.push(<PagerButton isActive={page === pagedResult.pageIndex}
                                        onClick={handleGoToSpecificPageFnFactory(page)}>{page}</PagerButton>);
    }

    return <div
        className="bg-[rgba(255,255,255,0.8)] flex flex-row justify-center w-full py-4 gap-x-2 text-deepKoamaru">
        {doesPreviousButtonExist && <PagerButton onClick={handleGoToFirstPage}>
            <VscArrowSmallLeft/>
        </PagerButton>}

        {numberButtons}

        {doesNextButtonExist && <PagerButton onClick={handleGoToLastPage}>
            <VscArrowSmallRight/>
        </PagerButton>}
    </div>
}

export {Pager};