import { Link } from "react-router-dom";
import { useQuery } from "react-query";
import { bookApiHelper } from "../helpers/apis/BookApiHelper";
import { BookCard } from "../components";

const EditorRecommendation = (): JSX.Element => {
  const imgUrl =
    "https://lh3.googleusercontent.com/oPhItNgqVzlAb0H_j8i2W0F3yIgmWCsrOv3nnH5yKCeiOdUjIXZwabzld9U8iWIE3DoeoFa5oRMYqssk8g=w215-h322-rw-no";

  const { isLoading, error, data } = useQuery(
    "recommendedBooks",
    () => bookApiHelper.getRecommended(),
    {
      staleTime: Infinity,
    }
  );

  return (
    <div className="w-full bg-white rounded p-4 shadow-sm shadow-slate-500 h-full">
      <h2 className="text-xl gradient-text font-bold mb-4">Đề cử</h2>

      <div className="flex flex-row flex-wrap md:mr-[-0.75rem]">
        {data &&
          data.map((elm, index) => (
            <BookCard
              key={elm.id}
              name={elm.name}
              shortDescription={elm.shortDescription}
              thumbnailImage={elm.thumbnailImage}
            />
          ))}
      </div>
    </div>
  );
};

export { EditorRecommendation };
