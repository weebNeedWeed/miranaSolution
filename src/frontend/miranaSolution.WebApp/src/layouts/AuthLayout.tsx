import {Outlet, useNavigate} from "react-router-dom";
import {LoadingScreen, ToastContainer} from "../containers";
import {useEffect} from "react";
import {useAccessToken} from "../helpers/hooks/useAccessToken";
import {getUserByAccessToken} from "../helpers/utilityFns/validateAccessToken";

const AuthLayout = (): JSX.Element => {
    const [accessToken, setAccessToken] = useAccessToken();
    const navigate = useNavigate();

    useEffect(() => {
        (async () => {
            if (accessToken && accessToken !== "") {
                const result = await getUserByAccessToken(accessToken);
                if (result === null) {
                    setAccessToken("");
                    return;
                }

                navigate("/");
            }
        })();
    }, [accessToken]);

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
