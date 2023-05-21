import { CssBaseline } from "@mui/material";
import { RouterProvider } from "react-router-dom";
import { router } from "./helpers/routes";
import { HelmetProvider } from "react-helmet-async";
import { SystemContextProvider } from "./contexts/SystemContext";
import { QueryClient, QueryClientProvider } from "react-query";

const queryClient = new QueryClient();

const App = (): JSX.Element => {
  return (
    <HelmetProvider>
      <CssBaseline/>
      <SystemContextProvider>
        <QueryClientProvider client={queryClient}>
          <RouterProvider router={router}/>
        </QueryClientProvider>
      </SystemContextProvider>
    </HelmetProvider>
  );
};

export { App };
