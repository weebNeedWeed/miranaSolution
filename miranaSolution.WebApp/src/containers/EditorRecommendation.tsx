import { Link } from "react-router-dom";
import { useQuery } from "react-query";
import { bookApiHelper } from "../helpers/apis/BookApiHelper";

type CardProps = {
  name: string;
  shortDescription: string;
  thumbnailImage: string;
};
const Card = (props: CardProps): JSX.Element => {
  return (
    <div className="md:w-[calc(calc(100%/2)-0.75rem)] md:mr-3 mb-3">
      <Link
        to={"/"}
        className="w-full bg-whiteChocolate p-2 flex cursor-pointer rounded shadow-sm shadow-darkVanilla border-2 border-solid hover:border-darkVanilla transition-all"
      >
        <div className="flex flex-row justify-center items-center">
          <img
            src={props.thumbnailImage}
            alt=""
            className="w-14 max-h-[84px] aspect-[2/3]"
          />

          <div className="flex flex-col ml-2 h-full items-start justify-start">
            <h4 className="text-sm font-semibold text-oldRose line-clamp-1 uppercase">
              {props.name}
            </h4>

            <p className="text-xs text-deepKoamaru line-clamp-3">
              {props.shortDescription}
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
  );
};

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
            <Card
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
