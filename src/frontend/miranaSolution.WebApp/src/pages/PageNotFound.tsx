import dog404 from "./../assets/dog404.jpg";
import React from "react";
import {Link} from "react-router-dom";

const PageNotFound = (): JSX.Element => {
    return <div className="w-full pt-12 flex flex-col items-center text-deepKoamaru min-h-[100vh]">
        <span className="text-8xl font-bold">
            404
        </span>
        <span className="text-lg">
            Trang không tồn tại
        </span>
        <img src={dog404} alt={"404 dog meme"} className="h-auto w-60 mt-2"/>

        <Link to={"/"} className="mt-4 py-1.5 px-2.5 bg-deepKoamaru text-white rounded font-light">
            Về trang chủ
        </Link>
    </div>
};

export {PageNotFound};

