import React from "react";
import { RouterProvider } from "react-router-dom";
import { router } from "./router";
import { QueryClient, QueryClientProvider } from "react-query";
import "./app.css";
import { SystemContextProvider } from "./contexts/SystemContext";

const queryClient = new QueryClient();

const App = (): JSX.Element => {
	return (
		<QueryClientProvider client={queryClient}>
			<SystemContextProvider>
				<RouterProvider router={router} />
			</SystemContextProvider>
		</QueryClientProvider>
	);
};

export { App };
