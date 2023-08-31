import {Link} from "react-router-dom";
import {Chapter} from "../helpers/models/catalog/books/Chapter";
import {useQuery} from "react-query";
import {bookApiHelper} from "../helpers/apis/BookApiHelper";
import {timeSince} from "../helpers/utilityFns/timeSince";
import {LatestChapter} from "../helpers/models/catalog/books/LatestChapter";

type TableRowProps = {
    latestChapter: LatestChapter
};
const TableRow = (props: TableRowProps) => {
    const {latestChapter} = props;

    return <tr className="odd:bg-white even:bg-slate-50 hover:bg-slate-50">
        <td className="hidden md:table-cell">
            <span className="text-sm text-deepKoamaru flex items-center">{latestChapter.genre}</span>
        </td>
        <td className="w-[50%] md:w-[30%] pr-8">
            <Link
                to={`/books/${latestChapter.bookSlug}`}
                className="w-full line-clamp-1 text-base text-deepKoamaru font-semibold hover:text-oldRose"
            >
                {latestChapter.bookName}
            </Link>
        </td>
        <td className="md:w-[30%] py-1">
            <Link
                to={`/books/${latestChapter.bookSlug}/chapters/${latestChapter.chapterIndex}`}
                className="w-full line-clamp-1 text-base text-deepKoamaru hover:text-oldRose"
            >
                Chương {latestChapter.chapterIndex}. {latestChapter.chapterName}
            </Link>
        </td>
        <td className="hidden md:table-cell">
            <p className="text-sm text-oldRose text-center">
                {latestChapter.authorName}
            </p>
        </td>
        <td className="hidden md:table-cell">
            <p className="text-sm text-oldRose text-right">{timeSince(latestChapter.updatedAt)}</p>
        </td>
    </tr>
}

const NewestChapters = (): JSX.Element => {
    const {isLoading, error, data} = useQuery(
        "latestChapters",
        () => bookApiHelper.getLatestChapters()
    );

    return (
        <div className="w-full bg-white rounded p-4 shadow-sm shadow-slate-500 h-full">
            <h2 className="text-xl gradient-text font-bold">Mới cập nhật</h2>

            <div className="w-full p-2">
                <table className="w-full">
                    <tbody>
                    {data && data.map(chapter => <TableRow latestChapter={chapter} key={chapter.id}/>)}
                    </tbody>
                </table>
            </div>
        </div>
    );
};

export {NewestChapters};
