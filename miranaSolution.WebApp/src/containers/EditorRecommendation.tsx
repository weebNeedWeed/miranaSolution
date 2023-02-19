import { Link } from "react-router-dom";

const EditorRecommendation = (): JSX.Element => {
  const imgUrl =
    "https://lh3.googleusercontent.com/oPhItNgqVzlAb0H_j8i2W0F3yIgmWCsrOv3nnH5yKCeiOdUjIXZwabzld9U8iWIE3DoeoFa5oRMYqssk8g=w215-h322-rw-no";

  return (
    <div className="w-full bg-white rounded p-4 shadow-sm shadow-slate-500">
      <h2 className="text-xl gradient-text font-bold mb-4">Đề cử</h2>

      <div className="flex flex-row flex-wrap mr-[-20px]">
        {Array.from(new Array(8)).map((elm, index) => (
          <div
            key={index}
            className="w-[calc(calc(100%/2)-20px)] mr-[20px] mb-3"
          >
            <Link
              to={"/"}
              className="w-full bg-whiteChocolate p-2 flex cursor-pointer rounded shadow-sm shadow-darkVanilla border-2 border-solid hover:border-darkVanilla transition-all"
            >
              <div className="flex flex-row justify-center items-center">
                <img src={imgUrl} alt="" className="w-14 max-h-[84px]" />

                <div className="flex flex-col ml-2 h-full items-start justify-start">
                  <h4 className="text-sm font-semibold text-oldRose line-clamp-1">
                    ĐỪNG KIẾM BẠN TRAI TRONG THÙNG RÁC
                  </h4>

                  <p className="text-xs text-deepKoamaru line-clamp-3">
                    Một người như Trì Tiểu Trì, thông qua phấn đấu cực lực, mà
                    từ hình thức địa ngục, xông thẳng lên con đường người chiến
                    thắng.
                  </p>

                  <div className="flex flex-row justify-start items-center">
                    <span className="text-xs bg-slate-300 text-deepKoamaru rounded-md p-1">
                      Hanh dong
                    </span>
                  </div>
                </div>
              </div>
            </Link>
          </div>
        ))}
      </div>
    </div>
  );
};

export { EditorRecommendation };
