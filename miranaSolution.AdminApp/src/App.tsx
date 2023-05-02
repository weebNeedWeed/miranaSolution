import { CssBaseline } from "@mui/material";
import { RouterProvider } from "react-router-dom";
import { router } from "./helpers/routes";
import { HelmetProvider } from "react-helmet-async";
import { SystemContextProvider } from "./contexts/SystemContext";

const App = (): JSX.Element => {
  return (
    <HelmetProvider>
      <CssBaseline/>
      <SystemContextProvider>
        <RouterProvider router={router}/>
      </SystemContextProvider>
    </HelmetProvider>
  );
};

export { App };
