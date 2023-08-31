import {Avatar, Pager} from "../components";
import {BiTime} from "react-icons/bi";
import {timeSince} from "../helpers/utilityFns/timeSince";
import {IoIosSend, IoMdArrowDropdown} from "react-icons/io";
import {AiFillLike} from "react-icons/ai";
import {BsFillReplyFill} from "react-icons/bs";
import {useQuery} from "react-query";
import {bookApiHelper} from "../helpers/apis/BookApiHelper";
import {useSearchParams} from "react-router-dom";
import {Comment} from "../helpers/models/catalog/comment/Comment";
import {userApiHelper} from "../helpers/apis/UserApiHelper";
import React, {useEffect, useRef, useState} from "react";
import {useAuthenticationContext} from "../contexts/AuthenticationContext";
import {useSystemContext} from "../contexts/SystemContext";
import {ToastVariant} from "../components/Toast";
import {useAccessToken} from "../helpers/hooks/useAccessToken";
import {useBaseUrl} from "../helpers/hooks/useBaseUrl";
import {v4 as uuidv4} from "uuid";
import {commentApiHelper} from "../helpers/apis/CommentApiHelper";
import clsx from "clsx";
import {FaTrash} from "react-icons/fa";
import {User} from "../helpers/models/catalog/user/User";

type DeleteButtonProps = {
    comment: Comment;
    onlyIcon?: boolean;
    refetch: () => void;
    userId: string;
}
const DeleteButton = React.memo((props: DeleteButtonProps): JSX.Element => {
    const {refetch, userId, comment} = props;
    const onlyIcon = props.onlyIcon ?? false;
    const authContext = useAuthenticationContext();
    const systemContext = useSystemContext();
    const [accessToken,] = useAccessToken();

    if (!authContext.state.isLoggedIn || authContext.state.user.id !== userId) {
        return <></>
    }

    const handleDeleteComment = async () => {
        try {
            await bookApiHelper.deleteComment(accessToken, {
                bookId: comment.bookId,
                commentId: comment.id
            });

            await refetch();

            systemContext.dispatch({
                type: "addToast",
                payload: {
                    variant: ToastVariant.Success,
                    title: "Xoá bình luận thành công."
                }
            });
        } catch (error: any) {
        }
    }

    return <button
        onClick={handleDeleteComment}
        className="flex flex-row items-center gap-x-1 hover:text-oldRose transition-all text-sm md:text-base">
        <FaTrash className="text-xs md:text-sm"/>
        {!onlyIcon && "Xoá"}
    </button>
});

type ReactionButtonProps = {
    commentId: number;
}
const ReactionButton = ({commentId}: ReactionButtonProps): JSX.Element => {
    const [isReacted, setIsReacted] = useState(false);

    const [accessToken, setAccessToken] = useAccessToken();
    const authContext = useAuthenticationContext();
    const systemContext = useSystemContext();

    const {data: countReactionResponse, refetch} = useQuery(
        ["reactions", commentId, authContext.state.user.id],
        () => commentApiHelper.countCommentReaction(commentId));

    useEffect(() => {
        (async () => {
            if (authContext.state.isLoggedIn) {
                const checkIsReacted = await commentApiHelper.checkUserIsReacted(accessToken, commentId);
                setIsReacted(checkIsReacted);
            }
        })();

    }, []);

    const handleReact = async () => {
        if (!authContext.state.isLoggedIn) {
            systemContext.dispatch({
                type: "addToast",
                payload: {
                    variant: ToastVariant.Warning,
                    title: "Vui lòng đăng nhập."
                }
            });
            return;
        }

        if (isReacted) {
            await commentApiHelper.deleteCommentReaction(accessToken, commentId);
            setIsReacted(false);
        } else {
            await commentApiHelper.createCommentReaction(accessToken, commentId);
            setIsReacted(true);
        }

        await refetch();
    }


    return <button
        onClick={handleReact}
        className={clsx("hover:text-oldRose font-semibold flex flex-row items-center gap-x-0.5 transition-all text-sm md:text-base", isReacted ? "text-oldRose" : "text-deepKoamaru")}>
        <AiFillLike className="text-sm md:text-base"/>
        {countReactionResponse?.totalReactions ?? 0}
    </button>;
}

type CommentFormProps = {
    bookId: number;
    parentId?: number;
    refetch?: () => void
};
const CommentForm = (props: CommentFormProps): JSX.Element => {
    const {bookId, parentId} = props;
    const [comment, setComment] = useState<string>("");
    const [focused, setFocused] = React.useState(false)
    const authContext = useAuthenticationContext();
    const systemContext = useSystemContext();
    const [accessToken, setAccessToken] = useAccessToken();

    const ref = useRef<HTMLTextAreaElement>(null);
    const baseUrl = useBaseUrl();

    useEffect(() => {
        if (focused && !authContext.state.isLoggedIn) {
            systemContext.dispatch({
                type: "addToast",
                payload: {
                    variant: ToastVariant.Warning,
                    title: "Vui lòng đăng nhập để bình luận."
                }
            });

            ref.current?.blur();
        }
    }, [focused]);

    const handleChange = (event: React.ChangeEvent<HTMLTextAreaElement>) => {
        setComment(event.target.value);
    }

    const handleSubmitForm = async (event: React.FormEvent<HTMLFormElement>) => {
        event.preventDefault();
        if (comment.trim().length === 0) {
            systemContext.dispatch({
                type: "addToast",
                payload: {
                    variant: ToastVariant.Warning,
                    title: "Vui lòng nhập bình luận."
                }
            });
            return;
        }

        try {
            await bookApiHelper.createComment(accessToken, bookId, {
                content: comment,
                parentId
            });
            setComment("");
            if (props.refetch) {
                props.refetch();
            }
        } catch (error: any) {
            systemContext.dispatch({
                type: "addToast",
                payload: {
                    variant: ToastVariant.Error,
                    title: error.message
                }
            });
        }
    }

    return <form onSubmit={handleSubmitForm} className="flex flex-row w-full gap-x-2 md:gap-x-4 mt-3 flex-wrap">
        {authContext.state.isLoggedIn && authContext.state.user.avatar !== "" ?
            <Avatar imageUrl={baseUrl + authContext.state.user.avatar} className="w-9 h-9 md:w-12 md:h-12"/> :
            <Avatar className="w-9 h-9 md:w-12 md:h-12"/>}

        <textarea
            ref={ref}
            onFocus={() => setFocused(true)}
            onBlur={() => setFocused(false)}
            value={comment}
            onChange={handleChange}
            placeholder="Nhập bình luận..."
            className="rounded-2xl grow resize-none px-3 py-1 border-oldRose border-2 border-solid outline-none"></textarea>

        <div className="w-full flex justify-end pt-2">
            <button type="submit"
                    className="text-sm md:text-base rounded-xl px-2 py-1 bg-oldRose flex flex-row items-center md:gap-x-1">
                <IoIosSend/>
                Bình luận
            </button>
        </div>
    </form>
}

type ReplyProps = {
    reply: Comment;
    parentRefetch: () => void
}
const Reply = ({reply, parentRefetch}: ReplyProps): JSX.Element => {
    const baseUrl = useBaseUrl();

    return <div className="w-full flex flex-row gap-x-2 border-t-[2px] pt-2 mt-2">
        {reply.userAvatar !== "" ?
            <Avatar imageUrl={baseUrl + reply.userAvatar} className="w-9 h-9 md:w-12 md:h-12 shrink-0"/> :
            <Avatar className="w-9 h-9 md:w-12 md:h-12 shrink-0"/>}
        <div className="flex flex-col w-full">
            <span className="font-semibold text-base max-w-2xl line-clamp-1">{reply.username}</span>
            <span className="md:mt-0.5">
                {reply.content}
            </span>

            <div className="flex flex-row items-center justify-between w-full mt-2">
                <div className="flex flex-row gap-x-3 md:gap-x-6">
                    <ReactionButton commentId={reply.id}/>

                    <span
                        className="flex flex-row items-center gap-x-1 font-normal text-xs">
                        <BiTime/>
                        {timeSince(reply.createdAt)}
                    </span>
                </div>

                <DeleteButton onlyIcon={true} comment={reply} refetch={parentRefetch} userId={reply.userId}/>
            </div>
        </div>
    </div>
}

type MainCommentProps = {
    comment: Comment;
    parentRefetch: () => void
}
const MainComment = ({comment, parentRefetch}: MainCommentProps): JSX.Element => {
    const [index, setIndex] = useState<number>(1);
    const [loadReply, setLoadReply] = useState<boolean>(false);

    const [replies, setReplies] = useState<Comment[][]>([]);
    const [totalReplies, setTotalReplies] = useState(0);
    const [totalPages, setTotalPages] = useState(0);

    const [retry, setRetry] = useState<boolean>(false);
    const [loadMore, setLoadMore] = useState(false)

    const replyPerLoad = 5;

    useEffect(() => {
        (async () => {
            try {
                const getSubCommentsResponse = await bookApiHelper
                    .getAllComments({
                        bookId: comment.bookId,
                        parentId: comment.id,
                        pageIndex: index,
                        pageSize: replyPerLoad,
                        asc: true
                    });

                const cloned = [...replies];
                cloned[index - 1] = getSubCommentsResponse.comments;

                setReplies(cloned);
                setTotalReplies(getSubCommentsResponse.totalComments);
                setTotalPages(getSubCommentsResponse.totalPages);

                const pageIndex = getSubCommentsResponse.pageIndex;
                const totalPages = getSubCommentsResponse.totalPages;

                setLoadMore(pageIndex < totalPages);

            } catch (error: any) {
            }
        })();
    }, [index, retry]);

    const baseUrl = useBaseUrl();

    const handleLoadReply = () => {
        setLoadReply(true);
    }

    const refetch = () => {
        if (totalPages === 0) {
            setRetry(!retry);
            return;
        }

        if (replies[index - 1].length <= replyPerLoad && index === totalPages) {
            setRetry(!retry);
            return;
        }

        if (index < totalPages) {
            for (let i = index + 1; i <= totalPages; ++i) {
                setTimeout(() => {
                    setIndex(i);
                }, 200);
            }
        }
    }

    const deleteRefetch = () => {
        if (index === 1) {
            setRetry(!retry);
            return;
        }

        setReplies([]);
        setIndex(1);
    }

    const handleLoadMore = () => {
        setIndex(index + 1);
    }

    return <div className="w-full flex flex-row gap-x-2 border-t-[2px] py-2">
        {comment.userAvatar !== "" ?
            <Avatar imageUrl={baseUrl + comment.userAvatar} className="w-9 h-9 md:w-12 md:h-12 shrink-0"/> :
            <Avatar className="w-9 h-9 md:w-12 md:h-12 shrink-0"/>}

        <div className="flex flex-col w-full">
            <span className="font-semibold text-base max-w-2xl line-clamp-1">{comment.username}</span>
            <span className="flex flex-row items-center gap-x-1 font-normal text-xs">
                <BiTime/>
                {timeSince(comment.createdAt)}
            </span>

            <span className="md:mt-2">
                {comment.content}
            </span>

            <span className="mt-2 flex flex-row justify-between items-center font-semibold w-full">
                {(totalReplies > 0 && !loadReply) &&
                    <button onClick={handleLoadReply} className="flex-row items-center text-base hidden md:flex">
                        Xem {totalReplies} câu trả lời
                        <IoMdArrowDropdown className="text-xl"/>
                    </button>}

                {(totalReplies > 0 && !loadReply) &&
                    <button onClick={handleLoadReply} className="flex flex-row items-center text-sm md:hidden">
                        {totalReplies} trả lời
                        <IoMdArrowDropdown className="text-lg"/>
                    </button>}

                <div className="flex flex-row gap-x-6 ml-auto">
                    <ReactionButton commentId={comment.id}/>

                    <button onClick={handleLoadReply}
                            className="flex flex-row items-center hover:text-oldRose transition-all text-sm md:text-base">
                        <BsFillReplyFill className="text-base md:text-lg"/>
                        Trả lời
                    </button>

                    <DeleteButton
                        comment={comment} userId={comment.userId} refetch={parentRefetch}/>
                </div>
            </span>

            {loadReply && <div className="flex flex-col w-full">
                {replies && replies.map((arr) => {
                    return arr.map(comment => <Reply parentRefetch={deleteRefetch} reply={comment} key={uuidv4()}/>)
                })}

                {loadMore && <button onClick={handleLoadMore}
                                     className="flex flex-row items-center font-semibold text-sm md:text-base pt-2">
                    Xem thêm trả lời
                    <IoMdArrowDropdown className="text-xl"/>
                </button>}

                <CommentForm refetch={refetch} bookId={comment.bookId} parentId={comment.id}/>
            </div>}
        </div>
    </div>
}

type CommentProps = {
    bookId: number;
    size?: number;
};
const CommentsSection = React.memo((props: CommentProps): JSX.Element => {
    const {bookId} = props;
    const [searchParams] = useSearchParams();

    const pageIndex = parseInt(searchParams.get("commentIndex") ?? "1", 10);
    const pageSize = parseInt(searchParams.get("commentSize") ?? (props.size?.toString() ?? "10"), 10);

    const {data, isLoading, error, refetch} = useQuery(
        ["comments", pageIndex, pageSize],
        () => bookApiHelper.getAllComments({
            bookId,
            pageIndex: pageIndex,
            pageSize: pageSize
        }), {
            staleTime: Infinity
        }
    );

    if (isLoading || error || !data) {
        return <div></div>
    }

    return <>
        <div className="font-semibold text-xl">
            Bình luận ({data.totalComments})
        </div>

        <CommentForm refetch={refetch} bookId={bookId}/>

        <div className="flex flex-col w-full mt-3">
            {data.comments.map((comment) => <MainComment parentRefetch={refetch} comment={comment} key={uuidv4()}/>)}
        </div>

        <div className="w-full">
            <Pager pageIndexKey={"commentIndex"} pageSizeKey={"commentSize"} pageIndex={data.pageIndex}
                   pageSize={data.pageSize} totalPages={data.totalPages}/>
        </div>
    </>
});

export {CommentsSection};