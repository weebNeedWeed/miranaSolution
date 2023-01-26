import React from "react";
import { Navbar } from "./layouts";
import { Outlet } from "react-router-dom";
import { LoadingContainer } from "./containers";

/**
 * @description Create app layout and add this as a route in routeList to that Link component can be used with data api
 * @description Without this Link cannot be used outside of RouterProvider
 */

const AppLayout = (): JSX.Element => {
	return (
		<div>
			<div>
				<Navbar />
			</div>

			<div>
				<Outlet />
			</div>

			<LoadingContainer />
		</div>
	);
};

export { AppLayout };
