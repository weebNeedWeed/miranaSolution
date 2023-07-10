import {Link} from "react-router-dom";

type BookCardProps = {
    name: string;
    shortDescription: string;
    thumbnailImage: string;
    slug?: string;
};
const BookCard = (props: BookCardProps): JSX.Element => {
    return (
        <div className="md:w-[calc(calc(100%/2)-0.75rem)] md:mr-3 mb-3">
            <Link
                to={`/books/${props.slug}`}
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

export {BookCard};