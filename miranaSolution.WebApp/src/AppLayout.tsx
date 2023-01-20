import React from "react";
import { Navbar, Header } from "./layouts/index";
import { Outlet } from "react-router-dom";

/**
 * @description Create app layout and add this as a route in routeList to that Link component can be used with data api
 * @description Without this Link cannot be used outside of RouterProvider
 */

const AppLayout = (): JSX.Element => {
	return (
		<div>
			<div>
				<Navbar />
				<Header />
			</div>

			<div>
				<Outlet />
			</div>
		</div>
	);
};

export { AppLayout };
