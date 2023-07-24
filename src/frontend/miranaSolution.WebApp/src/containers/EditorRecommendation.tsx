import {useQuery} from "react-query";
import {bookApiHelper} from "../helpers/apis/BookApiHelper";
import {BookCard} from "../components";

const EditorRecommendation = (): JSX.Element => {
    const {isLoading, error, data} = useQuery(
        "recommendedBooks",
        () => bookApiHelper.getRecommendedBooks()
    );

    return (
        <div className="w-full bg-white rounded p-4 shadow-sm shadow-slate-500 h-full">
            <h2 className="text-xl gradient-text font-bold mb-4">Nên đọc</h2>

            <div className="flex flex-row flex-wrap md:mr-[-0.75rem]">
                {data &&
                    data.map((elm, index) => (
                        <BookCard
                            key={elm.id}
                            name={elm.name}
                            shortDescription={elm.shortDescription}
                            thumbnailImage={elm.thumbnailImage}
                            slug={elm.slug}
                        />
                    ))}
            </div>
        </div>
    );
};

export {EditorRecommendation};
