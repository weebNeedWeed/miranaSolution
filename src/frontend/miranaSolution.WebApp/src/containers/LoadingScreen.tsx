import {useLayoutEffect} from "react";
import {Loading} from "../components";
import {useSystemContext} from "../contexts/SystemContext";
import {useLocation} from "react-router-dom";

const LoadingScreen = (): JSX.Element => {
    const {state, dispatch} = useSystemContext();
    const location = useLocation();

    const timeout = 3000;

    useLayoutEffect(() => {
        dispatch({type: "startLoading"});

        const temp = setTimeout(() => {
            dispatch({type: "endLoading"});
        }, timeout);

        return () => {
            if (temp)
                clearTimeout(temp);
        };
    }, [location]);

    return <Loading show={state.showLoading}/>;
};

export {LoadingScreen};
