import {useEffect} from "react";
import {GrClose} from "react-icons/gr";
import clsx from "clsx";

type DialogProps = {
    open: boolean;
    handleClose?: (event: React.MouseEvent<HTMLButtonElement>) => void;
    children: React.ReactNode;
    width: string;
    onBackdropClick?: (event: React.MouseEvent<HTMLDivElement>) => void;
};
const Dialog = (props: DialogProps): JSX.Element => {
    const {
        open,
        handleClose,
        width,
        children,
    } = props;

    return <div className={clsx(open ? "block" : "hidden", "z-40 absolute top-0 left-0 w-[100vw] h-[100vh]")}>
        <div className="fixed bg-[rgba(0,0,0,0.7)] w-full h-full"></div>
        <div className="fixed w-full mt-20 top-0 left-0 text-deepKoamaru px-8">
            <div className="mx-auto bg-white shadow-black shadow p-5 rounded max-w-full" style={{width: width}}>
                <div className="flex flex-col items-start justify-center">
                    <div className="w-full flex justify-end">
                        <button onClick={handleClose}>
                            <GrClose/>
                        </button>
                    </div>

                    <div className="mt-4 w-full">
                        {children}
                    </div>
                </div>
            </div>
        </div>
    </div>;
}

export {Dialog};