import { Router } from "@remix-run/router";
import { createBrowserRouter } from "react-router-dom";
import { RouteObject } from "react-router/dist/lib/context";

import { Home } from "../../pages";
import { AppLayout } from "../../AppLayout";
import { Test } from "../../pages/Test";

const routes: RouteObject[] = [
	{
		element: <AppLayout />,
		children: [
			{
				path: "/",
				element: <Home />,
				index: true,
			},
			{
				path: "/test",
				element: <Test />,
			},
		],
	},
];

const router: Router = createBrowserRouter(routes);

export { router };
