import {RouterProvider} from "react-router-dom";
import {router} from "./helpers/https/routing";
import {QueryClient, QueryClientProvider} from "react-query";
import {SystemContextProvider} from "./contexts/SystemContext";
import {AuthenticationContextProvider} from "./contexts/AuthenticationContext";

const queryClient = new QueryClient();

const App = (): JSX.Element => {
    return (
        <QueryClientProvider client={queryClient}>
            <SystemContextProvider>
                <AuthenticationContextProvider>
                    <RouterProvider router={router}/>
                </AuthenticationContextProvider>
            </SystemContextProvider>
        </QueryClientProvider>
    );
};

export {App};
