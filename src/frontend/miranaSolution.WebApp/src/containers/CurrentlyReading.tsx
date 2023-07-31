import {Link} from "react-router-dom";
import {MdOutlineClose} from "react-icons/md";

const ReadingCard = (): JSX.Element => {
    const imgUrl =
        "https://lh3.googleusercontent.com/oPhItNgqVzlAb0H_j8i2W0F3yIgmWCsrOv3nnH5yKCeiOdUjIXZwabzld9U8iWIE3DoeoFa5oRMYqssk8g=w215-h322-rw-no";

    const handleClick = (event: React.MouseEvent) => {
        event.preventDefault();

        console.log(11);
    };

    return (
        <Link
            to={"/"}
            className="relative group w-full bg-whiteChocolate p-2 flex cursor-pointer rounded shadow-sm shadow-darkVanilla border-2 border-solid hover:border-darkVanilla transition-all mb-3"
        >
            <div className="flex flex-row w-full items-center">
                <img src={imgUrl} alt="" className="aspect-[2/3] w-10"/>
                <div className="flex flex-col h-full justify-start items-start ml-2">
                    <h4 className="text-sm font-semibold text-oldRose line-clamp-1 mt-1">
                        ĐỪNG KIẾM BẠN TRAI TRONG THÙNG RÁC
                    </h4>

                    <span className="text-sm text-deepKoamaru mt-1">Số chương: {0}</span>
                </div>
            </div>

            <button
                onClick={handleClick}
                className="md:hidden md:group-hover:block absolute top-0 right-0 mt-2 mr-2 text-md text-deepKoamaru font-bold"
            >
                <MdOutlineClose/>
            </button>
        </Link>
    );
};

const CurrentlyReading = (): JSX.Element => {
    return (
        <div className="w-full bg-white rounded p-4 shadow-sm shadow-slate-500 h-full">
            <h2 className="text-xl gradient-text font-bold mb-4">Đang đọc</h2>

            <div className="flex flex-col justify-start items-center">
                {Array.from(new Array(5)).map((elm, index) => (
                    <ReadingCard key={index}/>
                ))}
            </div>
        </div>
    );
};

export {CurrentlyReading};
