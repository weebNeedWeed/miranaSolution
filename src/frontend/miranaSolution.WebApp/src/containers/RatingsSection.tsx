import {Avatar, Pager, Rating} from "../components";
import {BiTime} from "react-icons/bi";
import {timeSince} from "../helpers/utilityFns/timeSince";
import React from "react";
import {useQuery} from "react-query";
import {useSearchParams} from "react-router-dom";
import {bookApiHelper} from "../helpers/apis/BookApiHelper";
import {BookRating} from "../helpers/models/catalog/books/BookRating";
import {userApiHelper} from "../helpers/apis/UserApiHelper";
import {useBaseUrl} from "../helpers/hooks/useBaseUrl";
import {Book} from "../helpers/models/catalog/books/Book";

type RatingBlockProps = {
    rating: BookRating
}
const RatingBlock = (props: RatingBlockProps): JSX.Element => {
    const {rating} = props;
    const baseUrl = useBaseUrl();
    const {data: userData} = useQuery(
        ["user", rating.userId],
        () => userApiHelper.getUserProfileById(rating.userId), {
            staleTime: Infinity
        });

    if (!userData) {
        return <div></div>
    }

    return <div className="flex flex-row gap-x-2 border-b-[2px] py-2">
        {userData.avatar !== "" ?
            <Avatar imageUrl={baseUrl + userData.avatar} className="w-12 h-12 shrink-0"/> :
            <Avatar className="w-12 h-12 shrink-0"/>}

        <div className="flex flex-col grow">
            <span className="font-semibold text-base max-w-2xl line-clamp-1">{userData.userName}</span>
            <div className="flex flex-row items-center gap-x-4">
                <span className="flex flex-row items-center gap-x-1">
                    <Rating value={rating.star}/>

                    <span className="font-normal text-sm">{rating.star}</span>
                </span>

                <span className="flex flex-row items-center gap-x-1 font-normal text-xs">
                    <BiTime/>
                    {timeSince(rating.createdAt)}
                </span>
            </div>

            <span className="mt-2">
                {rating.content}
            </span>
        </div>
    </div>
}

type RatingsSectionProps = {
    book: Book
}

const RatingsSection = ({book}: RatingsSectionProps): JSX.Element => {
    const pageIndexKey = "ratingIndex";
    const pageSizeKey = "ratingSize";

    const [searchParams] = useSearchParams();

    const pageIndex = parseInt(searchParams.get(pageIndexKey) ?? "1", 10);
    const pageSize = parseInt(searchParams.get(pageSizeKey) ?? "10", 10);

    const {data: response} = useQuery(
        ["ratings", pageIndex, pageSize],
        () => bookApiHelper.getAllRatings(book.id, undefined, pageIndex, pageSize)
    );

    const {data: ratingsOverview} = useQuery(
        ["ratingsOverview", book.id],
        () => bookApiHelper.getRatingsOverview(book.id)
    );

    if (!response) {
        return <></>
    }

    return <div className="flex flex-row w-full h-full">
        <div className="mr-4 w-full flex flex-col min-h-[650px]">
            <span className="font-semibold text-lg mb-2">{response.totalRatings} đánh giá</span>

            {response.bookRatings.map((rating, index) => <RatingBlock rating={rating} key={index}/>)}

            <div className="mt-auto">
                <Pager pageIndex={response.pageIndex} pageSize={response.pageSize} totalPages={response.totalPages}
                       pageSizeKey={pageSizeKey}
                       pageIndexKey={pageIndexKey}/>
            </div>
        </div>

        <div
            className="flex flex-col bg-oldRose rounded-md p-5 min-w-[300px] h-full justify-start items-start">
            <h4 className="font-semibold text-xl">
                Tổng quan
            </h4>

            {ratingsOverview && <div className="flex flex-col mt-1 pl-2">
                {Array.from(new Array(5)).map((_, index) => <div
                    key={index}
                    className="flex flex-row gap-x-2 items-center text-base font-normal">
                    <Rating value={5 - index} width={17} spacing={2}/>

                    <span>
                        {ratingsOverview.ratingsByStar[5 - index]} đánh giá
                    </span>
                </div>)}
            </div>}
        </div>
    </div>
}

export {RatingsSection};