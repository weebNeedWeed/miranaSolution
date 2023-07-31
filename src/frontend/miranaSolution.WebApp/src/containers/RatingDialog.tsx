import {Dialog, Rating} from "../components";
import React, {useEffect, useState} from "react";
import clsx from "clsx";
import {bookApiHelper} from "../helpers/apis/BookApiHelper";
import {useAccessToken} from "../helpers/hooks/useAccessToken";
import {useAuthenticationContext} from "../contexts/AuthenticationContext";
import {useSystemContext} from "../contexts/SystemContext";
import {ToastVariant} from "../components/Toast";
import {Book} from "../helpers/models/catalog/books/Book";
import {BookRating} from "../helpers/models/catalog/books/BookRating";
import {FaTrash} from "react-icons/fa";

type RatingDialogProps = {
    open: boolean;
    handleClose: () => void;
    book: Book;
}
const RatingDialog = (props: RatingDialogProps): JSX.Element => {
    const {open, handleClose, book} = props;
    const [star, setStar] = useState(0);
    const [content, setContent] = useState("");

    const [accessToken,] = useAccessToken();
    const authContext = useAuthenticationContext();
    const systemContext = useSystemContext();

    const [userRating, serUserRating] = useState<BookRating>();

    useEffect(() => {
        (async () => {
            if (authContext.state.isLoggedIn) {
                try {
                    const ratingsResponse = await bookApiHelper.getAllRatings(book.id, authContext.state.user.id);
                    const ratings = ratingsResponse.bookRatings;
                    if (ratings.length > 0) {
                        serUserRating(ratings[0]);
                    }

                    setStar(ratings[0].star);
                    setContent(ratings[0].content);
                } catch (error: any) {
                }
            }
        })();

    }, [authContext.state.isLoggedIn]);


    const handleChangeStar = (_: number) => setStar(_);
    const handleChangeContent = (event: React.ChangeEvent<HTMLTextAreaElement>) => {
        setContent(event.target.value);
    }

    const handleDeleteRating = async () => {
        if (typeof userRating === "undefined") {
            return;
        }

        try {
            await bookApiHelper.deleteRating(accessToken, book.id);

            setStar(0);
            setContent("");

            serUserRating(undefined);

            systemContext.dispatch({
                type: "addToast",
                payload: {
                    variant: ToastVariant.Success,
                    title: "Xoá đánh giá thành công."
                }
            });
        } catch (error: any) {

        }
    }

    const handleSubmit = async (event: React.FormEvent<HTMLFormElement>) => {
        event.preventDefault();

        if (!authContext.state.isLoggedIn) {
            systemContext.dispatch({
                type: "addToast",
                payload: {
                    variant: ToastVariant.Warning,
                    title: "Vui lòng đăng nhập để đánh giá."
                }
            });
            return;
        }

        try {
            let rating: BookRating;

            if (typeof userRating !== "undefined") {
                rating = await bookApiHelper.updateRating(accessToken, props.book.id, {
                    content: content.trim(),
                    star
                });
                systemContext.dispatch({
                    type: "addToast",
                    payload: {
                        variant: ToastVariant.Success,
                        title: "Cập nhật đánh giá thành công."
                    }
                });
                serUserRating(rating);
                return;
            }

            rating = await bookApiHelper.createRating(accessToken, props.book.id, {
                content: content.trim(),
                star
            });

            serUserRating(rating);

            systemContext.dispatch({
                type: "addToast",
                payload: {
                    variant: ToastVariant.Success,
                    title: "Tạo đánh giá thành công."
                }
            });
        } catch (error: any) {
            systemContext.dispatch({
                type: "addToast",
                payload: {
                    variant: ToastVariant.Error,
                    title: error.message
                }
            });
        }
    };

    return <Dialog handleClose={handleClose} open={open} width="400px">
        <form onSubmit={handleSubmit} className="flex flex-col justify-start items-center text-deepKoamaru">
            <h4 className="text-oldRose text-xl font-bold text-left w-full mb-2">
                Đánh giá
            </h4>

            <p className="text-base w-full text-left">
                Bạn cảm thấy truyện thế nào ?
            </p>

            <div className="flex flex-row w-full justify-start items-center gap-x-1 mb-2">
                <Rating width={17} readonly={false} size={5} value={star} onChange={handleChangeStar}/>
                <span className="text-sm">
                    <span className="font-semibold">{star}/</span>
                    5
                </span>
            </div>

            <p className="text-base w-full text-left">
                Cảm nghĩ của bạn về truyện:
            </p>

            <textarea value={content}
                      placeholder="Nhập cảm nghĩ của bạn..."
                      onChange={handleChangeContent}
                      rows={4}
                      className="mt-1 resize-none rounded-lg w-full outline-none border-2 border-oldRose p-2">
            </textarea>

            <div className="flex flex-row justify-between items-end w-full mt-4">
                <button
                    type="submit"
                    className={clsx(star == 0 && "bg-[rgba(48,54,89,0.5)]", "rounded bg-deepKoamaru py-1.5 px-3 text-white flex justify-center items-center gap-x-2 text-sm md:text-base self-start")}>
                    {typeof userRating === "undefined" ? "Tạo đánh giá" : "Cập nhật"}
                </button>

                <button type="button"
                        onClick={handleDeleteRating}
                        className={clsx(typeof userRating === "undefined" && "hidden", "text-red-500 text-lg mr-1 mb-1.5")}>
                    <FaTrash/>
                </button>
            </div>
        </form>
    </Dialog>
}

export {RatingDialog};