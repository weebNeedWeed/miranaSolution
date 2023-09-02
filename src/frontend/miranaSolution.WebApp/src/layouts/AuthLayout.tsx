import {Outlet, useNavigate} from "react-router-dom";
import {LoadingScreen, ToastContainer} from "../containers";
import {useEffect, useLayoutEffect} from "react";
import {useAccessToken} from "../helpers/hooks/useAccessToken";
import {getUserByAccessToken} from "../helpers/utilityFns/validateAccessToken";

const AuthLayout = (): JSX.Element => {
    const [accessToken, setAccessToken] = useAccessToken();
    const navigate = useNavigate();

    useLayoutEffect(() => {
        (async () => {
            if (accessToken.trim()) {
                const result = await getUserByAccessToken(accessToken);
                if (result === null) {
                    setAccessToken("");
                    return;
                }

                navigate("/");
            }
        })();
    }, []);

    return (
        <div className="flex w-full justify-center min-h-[100vh]">
            <div className="my-20 w-[min(400px,80vw)]">
                <Outlet/>
            </div>

            <LoadingScreen/>

            <ToastContainer/>
        </div>
    );
};

export {AuthLayout};
