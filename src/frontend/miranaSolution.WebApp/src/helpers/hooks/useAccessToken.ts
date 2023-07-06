import {useLocalStorage} from "./useLocalStorage";

export const useAccessToken = () => {
    const [accessToken, setAccessToken] = useLocalStorage("accessToken", "");

    return [accessToken, setAccessToken] as [typeof accessToken, typeof setAccessToken];
}