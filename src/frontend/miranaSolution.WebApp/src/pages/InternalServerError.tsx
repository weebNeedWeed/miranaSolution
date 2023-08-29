import dog404 from "../assets/dog404.jpg";
import {Link} from "react-router-dom";
import React from "react";

const InternalServerError = (): JSX.Element => {
    return <div className="w-full pt-12 flex flex-col items-center text-deepKoamaru min-h-[100vh]">
        <span className="text-8xl font-bold">
            500
        </span>
        <span className="text-lg">
            Lỗi không xác định
        </span>
        <img src={dog404} alt={"404 dog meme"} className="h-auto w-60 mt-2"/>

        <Link to={"/"} className="mt-4 py-1.5 px-2.5 bg-deepKoamaru text-white rounded font-light">
            Về trang chủ
        </Link>
    </div>
};

export {InternalServerError};