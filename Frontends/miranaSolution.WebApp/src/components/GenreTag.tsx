import clsx from "clsx";

type GenreTagProps = {
	genre: string;
	className?: string;
};

const GenreTag = (props: GenreTagProps): JSX.Element => {
	const {genre} = props;

	const className: string = clsx(
		"bg-oldRose mb-4 py-1 px-2 cursor-pointer border-b-2 border-solid border-deepKoamaru text-deepKoamaru text-sm font-normal",
		props.className ?? "",
	);

	return <span className={className}>{genre}</span>;
};

export { GenreTag };
