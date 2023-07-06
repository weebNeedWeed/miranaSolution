import {Avatar} from "../../components";
import {useAuthenticationContext} from "../../contexts/AuthenticationContext";
import React from "react";
import {Link} from "react-router-dom";
import {BsPencil} from "react-icons/bs";

const UserProfile = (): JSX.Element => {
    const {state, dispatch} = useAuthenticationContext();

    return <div className="flex flex-col items-center justify-start mx-auto md:w-[400px] max-w-full">
        <span className="w-full flex flex-col justify-start mt-4 items-center">
            <Avatar className="w-28 h-28 sm:w-40 sm:h-40"/>

            <span className="font-bold text-xl mt-2">{state.user.firstName} {state.user.lastName}</span>
            <span className="font-normal text-sm">@{state.user.userName}</span>
        </span>

        <Link to=""
              className="rounded bg-deepKoamaru py-2.5 px-3 mt-4 text-white flex justify-center items-center gap-x-2 text-sm md:text-base">
            <BsPencil/> <span>Sửa thông tin</span>
        </Link>

        <ul className="mt-12 self-start">
            <li className="flex flex-row gap-x-2 items-center">
                <span className="w-4 h-4 bg-oldRose block"></span>
                <span className="text-lg font-semibold">
                    Đã đọc: <span className="text-oldRose">0</span> truyện, <span
                    className="text-oldRose">0</span> chương
                        </span>
            </li>

            <li className="flex flex-row gap-x-2 items-center">
                <span className="w-4 h-4 bg-oldRose block"></span>
                <span className="text-lg font-semibold">
                    Bình luận: <span className="text-oldRose">0</span>
                </span>
            </li>
        </ul>
    </div>
}

export {UserProfile};