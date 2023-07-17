import clsx from "clsx";
import catAvatar from "./../assets/cat_avatar.jpg";

type AvatarProps = {
    imageUrl?: string;
    className?: string;
};
const Avatar = (props: AvatarProps): JSX.Element => {
    const {imageUrl, className} = props;
    const hasImageUrl = typeof imageUrl !== "undefined";

    console.log(props)

    return <span
        style={{backgroundImage: hasImageUrl ? `url('${imageUrl}')` : `url('${catAvatar}')`}}
        className={clsx(className, "drop-shadow-md bg-slate-500 flex justify-center items-center rounded-full bg-no-repeat bg-center bg-cover")}>
    </span>
};

export {Avatar};