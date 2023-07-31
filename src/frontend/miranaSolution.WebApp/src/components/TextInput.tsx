import clsx from "clsx";

type TextInputProps = {
    value?: string;
    onChange?: (event: React.ChangeEvent<HTMLInputElement>) => void;
    icon?: React.ReactNode;
    placeholder?: string;
    type?: string;
    label?: string;
    className?: string
};
const TextInput = (props: TextInputProps): JSX.Element => {
    const {className, label, value, onChange, icon, type, placeholder} = props;

    return <>
        <label
            className="font-semibold text-sm text-slate-600"
        >
            {label || ""}
        </label>
        <div
            className={clsx(className || "", "text-slate-400 flex flex-row w-full justify-start items-center py-2 px-1 border-slate-300 border-b-2 border-solid gap-x-2 text-sm")}>
            <span>
                {icon || ""}
            </span>
            <input
                type={typeof type === "undefined" ? "text" : type}
                className=" grow focus:border-0 outline-none bg-transparent"
                placeholder={placeholder || ""}
                value={value || ""}
                onChange={onChange || (() => ({}))}
            />
        </div>
    </>;
}

export {TextInput};