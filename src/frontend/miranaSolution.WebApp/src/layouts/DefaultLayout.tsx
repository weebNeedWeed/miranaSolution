import {Footer, Navbar} from "../components";
import {Outlet, useNavigate} from "react-router-dom";
import {LoadingScreen, ToastContainer} from "../containers";
import {useAccessToken} from "../helpers/hooks/useAccessToken";
import {getUserByAccessToken} from "../helpers/utilityFns/validateAccessToken";
import {useEffect} from "react";
import {useSystemContext} from "../contexts/SystemContext";
import {ToastMessage, ToastVariant} from "../components/Toast";
import {useAuthenticationContext} from "../contexts/AuthenticationContext";

// Here's the default layout of the application
// It's used for all pages except Auth pages(which has different layout)

const DefaultLayout = (): JSX.Element => {
    const [accessToken, setAccessToken] = useAccessToken();
    const navigate = useNavigate();
    const {state: systemState, dispatch: systemDispatch} = useSystemContext();
    const {state: authenticationState, dispatch: authenticationDispatch} = useAuthenticationContext();

    useEffect(() => {

        (async () => {
            if (accessToken && accessToken !== "") {
                const result = await getUserByAccessToken(accessToken);

                if (result === null) {
                    setAccessToken("");
                    systemDispatch({
                        type: "addToast", payload: {
                            title: "Phiên đã hết hạn, vui lòng đăng nhập lại.",
                            variant: ToastVariant.Error
                        } as ToastMessage
                    });
                    navigate("/auth/login");
                    return;
                }

                authenticationDispatch({
                    type: "setUserData", payload: {
                        user: result.user
                    }
                });

                authenticationDispatch({
                    type: "setLoginStatus", payload: {
                        status: true
                    }
                });
            }
        })();

    }, [accessToken]);

    return (
        <div>
            <div>
                <Navbar/>
            </div>

            <div>
                <Outlet/>
            </div>

            <Footer/>

            <LoadingScreen/>

            <ToastContainer/>
        </div>
    );
};

export {DefaultLayout};
