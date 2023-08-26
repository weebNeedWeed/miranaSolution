import {RouterProvider} from "react-router-dom";
import {router} from "./helpers/https/routing";
import {QueryClient, QueryClientProvider} from "react-query";
import {SystemContextProvider} from "./contexts/SystemContext";
import {AuthenticationContextProvider} from "./contexts/AuthenticationContext";
import React from "react";
import {Helmet} from "react-helmet";
import logo from "./assets/favicon.svg";

const queryClient = new QueryClient();

const App = (): JSX.Element => {
    return (
        <QueryClientProvider client={queryClient}>
            <SystemContextProvider>
                <AuthenticationContextProvider>
                    <Helmet>
                        <title>
                            Home | Mirana Readers
                        </title>
                        <link rel="icon" href={logo}/>
                    </Helmet>
                    <RouterProvider router={router}/>
                </AuthenticationContextProvider>
            </SystemContextProvider>
        </QueryClientProvider>
    );
};

export {App};
