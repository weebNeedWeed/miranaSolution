import {Link} from "react-router-dom";
import {Chapter} from "../helpers/models/catalog/books/Chapter";
import {useQuery} from "react-query";
import {bookApiHelper} from "../helpers/apis/BookApiHelper";
import {timeSince} from "../helpers/utilityFns/timeSince";

type TableRowProps = {
    chapter: Chapter
};
const TableRow = (props: TableRowProps) => {
    const {chapter} = props;

    const {isLoading, error, data} = useQuery(
        ["book", chapter.bookId],
        () => bookApiHelper.getBookById(chapter.bookId)
    );

    if (isLoading || error || !data) {
        return <></>;
    }

    return <tr className="odd:bg-white even:bg-slate-50 hover:bg-slate-50">
        <td className="hidden md:table-cell">
            <span className="text-sm text-deepKoamaru flex items-center">{data.genres[0]}</span>
        </td>
        <td className="w-[50%] md:w-[30%] pr-8">
            <Link
                to={`/books/${data.slug}`}
                className="w-full line-clamp-1 text-base text-deepKoamaru font-semibold hover:text-oldRose"
            >
                {data.name}
            </Link>
        </td>
        <td className="md:w-[30%] py-1">
            <Link
                to={`/books/${data.slug}/chapters/${chapter.index}`}
                className="w-full line-clamp-1 text-base text-deepKoamaru hover:text-oldRose"
            >
                Chương {chapter.index}. {chapter.name}
            </Link>
        </td>
        <td className="hidden md:table-cell">
            <p className="text-sm text-oldRose text-center">
                {data.authorName}
            </p>
        </td>
        <td className="hidden md:table-cell">
            <p className="text-sm text-oldRose text-right">{timeSince(chapter.updatedAt)}</p>
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
                    {data && data.map(chapter => <TableRow chapter={chapter} key={chapter.id}/>)}
                    </tbody>
                </table>
            </div>
        </div>
    );
};

export {NewestChapters};
