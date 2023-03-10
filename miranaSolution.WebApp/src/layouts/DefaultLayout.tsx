import { Footer, Navbar } from ".";
import { Outlet } from "react-router-dom";
import { LoadingScreen } from "../containers";

/**
 * @description Create app layout and add this as a route in routeList to that Link component can be used with data api
 * @description Without this Link cannot be used outside of RouterProvider
 */

const DefaultLayout = (): JSX.Element => {
  return (
    <div>
      <div>
        <Navbar />
      </div>

      <div>
        <Outlet />
      </div>

      <Footer />

      <LoadingScreen />
    </div>
  );
};

export { DefaultLayout };
