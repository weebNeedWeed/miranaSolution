import {Outlet, useNavigate} from "react-router-dom";
import {useBaseUrl} from "../helpers/hooks/useBaseUrl";
import {useLayoutEffect} from "react";
import axios from "axios";

const GlobalChecking = (): JSX.Element => {
    const navigate = useNavigate();
    const baseUrl = useBaseUrl();
    useLayoutEffect(() => {
        const healthCheckEndpoint = `${baseUrl}_health`;
        axios.get(healthCheckEndpoint, {timeout: 2000})
            .then(response => {
                const text: string = response.data;
                if (text.toLowerCase() === "unhealthy") {
                    navigate("/500");
                }
            })
            .catch((error) => {
                navigate("/500");
            });
    }, []);
    return <Outlet></Outlet>;
}

export {GlobalChecking};