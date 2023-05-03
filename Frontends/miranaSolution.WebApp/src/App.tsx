import { RouterProvider } from "react-router-dom";
import { router } from "./helpers/https/routing";
import { QueryClient, QueryClientProvider } from "react-query";
import { SystemContextProvider } from "./contexts/SystemContext";
import { useLocalStorage } from "./helpers/hooks/useLocalStorage";
import jwt_decode, { JwtPayload } from "jwt-decode";

const queryClient = new QueryClient();

const App = (): JSX.Element => {
  const [accessToken, setAccessToken] = useLocalStorage("AccessToken", "");
  if (accessToken.trim() !== "") {
    const decoded = jwt_decode<JwtPayload>(accessToken);
    const exp = decoded.exp!;
    const currentTime = Date.now() / 1000;

    if (exp <= currentTime) {
      setAccessToken("");
    }
  }

  return (
    <QueryClientProvider client={queryClient}>
      <SystemContextProvider>
        <RouterProvider router={router} />
      </SystemContextProvider>
    </QueryClientProvider>
  );
};

export { App };
