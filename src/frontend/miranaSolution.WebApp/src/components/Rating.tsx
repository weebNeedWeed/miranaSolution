import {AiFillStar, AiOutlineStar} from "react-icons/ai";

type StarProps = {
    active?: boolean,
    readonly?: boolean,
    onChange?: (value: number) => void,
    value: number;
    style?: React.CSSProperties
}
const Star = (props: StarProps): JSX.Element => {
    const active = props.active ?? false;
    const readonly = props.readonly ?? true;
    const style = props.style ?? {};
    const {value} = props;

    let onChange = (value: number) => {
    }
    if (typeof props.onChange !== "undefined") {
        onChange = props.onChange;
    }

    const handleClick = () => {
        onChange(value);
    }

    return <button type="button" style={style} className="text-[#faaf00]" disabled={readonly} onClick={handleClick}>
        {active ? <AiFillStar/> : <AiOutlineStar/>}
    </button>
}

type RatingProps = {
    spacing?: number,
    value: number,
    size?: number,
    readonly?: boolean,
    onChange?: (value: number) => void,
    width?: number;
};

const Rating = (props: RatingProps): JSX.Element => {
    const spacing = props.spacing ?? 0;
    const width = props.width ?? 16;
    const size = props.size ?? 5;
    const value = props.value % (size + 1);
    const readonly = props.readonly ?? true;
    const onChange = props.onChange

    return <div style={{gap: spacing}} className="flex flex-row">
        {Array.from(new Array(size)).map((_, index) => <Star
            style={{
                fontSize: width
            }}
            value={index + 1}
            readonly={readonly}
            key={index}
            active={(index + 1) <= value}
            onChange={onChange}/>)}
    </div>;
}

export {Rating};