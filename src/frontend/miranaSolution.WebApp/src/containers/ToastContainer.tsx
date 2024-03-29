import {Toast} from "../components";
import {useSystemContext} from "../contexts/SystemContext";
import React from "react";
import {AnimatePresence} from "framer-motion";
import clsx from "clsx";

const ToastContainer = (): JSX.Element => {
    const {state, dispatch} = useSystemContext();
    const removeToastById = (id: string) => () => {
        dispatch({type: "removeToast", payload: id});
    };

    return <div
        className={clsx(state.toast.length > 0 && "py-3", "pr-3 fixed top-0 right-0 w-[min(350px,100%)] z-50 flex flex-col gap-y-4")}>
        <AnimatePresence>
            {state.toast.slice(0, 5).map((t, index) => <Toast key={t.id} toast={t}
                                                              removeToastById={removeToastById(t.id!)}/>)}
        </AnimatePresence>
    </div>;
};

export {ToastContainer};