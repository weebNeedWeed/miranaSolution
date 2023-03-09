import { Link } from "react-router-dom";

const NewestChapters = (): JSX.Element => {
  return (
    <div className="w-full bg-white rounded p-4 shadow-sm shadow-slate-500 h-full">
      <h2 className="text-xl gradient-text font-bold">Mới cập nhật</h2>

      <div className="w-full p-2">
        <table className="w-full">
          <tbody>
            <tr className="odd:bg-white even:bg-slate-50 hover:bg-slate-50">
              <td className="hidden md:table-cell">
                <p className="text-sm text-deepKoamaru">Đồng nhân</p>
              </td>
              <td className="w-[50%] md:w-[30%] pr-8 py-4">
                <Link
                  to={"/"}
                  className="w-full line-clamp-1 text-base text-deepKoamaru font-semibold hover:text-oldRose"
                >
                  Trảm Yêu Trừ Ma, Bắt Đầu Thu Hoạch Được Sáu Mươi Năm Công Lực
                </Link>
              </td>
              <td className="md:w-[30%]">
                <Link
                  to={"/"}
                  className="w-full line-clamp-1 text-base text-deepKoamaru hover:text-oldRose"
                >
                  Chương 415. Đi theo ta đi
                </Link>
              </td>
              <td className="hidden md:table-cell">
                <p className="text-sm text-oldRose text-center">
                  Đại Hải Thuyền
                </p>
              </td>
              <td className="hidden md:table-cell">
                <p className="text-sm text-oldRose text-right">vừa mới</p>
              </td>
            </tr>
            <tr className="odd:bg-white even:bg-slate-50 hover:bg-slate-50 py-4">
              <td className="hidden md:table-cell">
                <p className="text-sm text-deepKoamaru">Đồng nhân</p>
              </td>
              <td className="w-[50%] md:w-[30%] pr-8">
                <Link
                  to={"/"}
                  className="w-full line-clamp-1 text-base text-deepKoamaru font-semibold hover:text-oldRose"
                >
                  Trảm Yêu Trừ Ma, Bắt Đầu Thu Hoạch Được Sáu Mươi Năm Công Lực
                </Link>
              </td>
              <td className="md:w-[30%]">
                <Link
                  to={"/"}
                  className="w-full line-clamp-1 text-base text-deepKoamaru hover:text-oldRose"
                >
                  Chương 415. Đi theo ta đi
                </Link>
              </td>
              <td className="hidden md:table-cell">
                <p className="text-sm text-oldRose text-center">
                  Đại Hải Thuyền
                </p>
              </td>
              <td className="hidden md:table-cell">
                <p className="text-sm text-oldRose text-right">vừa mới</p>
              </td>
            </tr>
          </tbody>
        </table>
      </div>
    </div>
  );
};

export { NewestChapters };
