import { Link } from "react-router-dom";

const MostReadingCard = (): JSX.Element => {
  const imgUrl =
    "https://lh3.googleusercontent.com/oPhItNgqVzlAb0H_j8i2W0F3yIgmWCsrOv3nnH5yKCeiOdUjIXZwabzld9U8iWIE3DoeoFa5oRMYqssk8g=w215-h322-rw-no";

  return (
    <Link
      to={"/"}
      className="group w-full sm:w-[calc(calc(100%/2)-20px)] sm:ml-[20px] md:w-[calc(calc(100%/4)-20px)] lg:w-[calc(calc(100%/6)-20px)] bg-whiteChocolate p-2 cursor-pointer rounded shadow-sm shadow-darkVanilla border-2 border-solid hover:border-darkVanilla transition-all mb-[20px]"
    >
      <div className="flex flex-col items-center">
        <img src={imgUrl} alt="" className="aspect-[2/3] w-full" />
        <div className="flex flex-col h-full justify-start items-start">
          <h4 className="text-sm font-semibold text-oldRose line-clamp-2 h-10 mt-1">
            ĐỪNG KIẾM BẠN TRAI TRONG THÙNG RÁC
          </h4>

          <span className="text-sm text-deepKoamaru mt-1">Lượt đọc: {0}</span>
        </div>
      </div>

      <button className="md:hidden md:group-hover:block absolute top-0 right-0 mt-2 mr-2 text-md text-deepKoamaru font-bold"></button>
    </Link>
  );
};

const MostReading = (): JSX.Element => {
  return (
    <div className="w-full bg-white rounded p-4 shadow-sm shadow-slate-500 h-full">
      <h2 className="text-xl gradient-text font-bold">Đọc nhiều</h2>

      <div className="flex flex-row flex-wrap mt-4 sm:ml-[-20px] p-2">
        {Array.from(new Array(20)).map((elm, index) => (
          <MostReadingCard key={index} />
        ))}
      </div>
    </div>
  );
};

export { MostReading };
