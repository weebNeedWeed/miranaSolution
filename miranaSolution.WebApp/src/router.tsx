import { Router } from "@remix-run/router";
import React from "react";
import { createBrowserRouter } from "react-router-dom";
import { RouteObject } from "react-router/dist/lib/context";

import { Home } from "./pages";
import { AppLayout } from "./AppLayout";

const routes: RouteObject[] = [
	{
		element: <AppLayout />,
		children: [
			{
				path: "/",
				element: <Home />,
				index: true,
			},
		],
	},
];

const router: Router = createBrowserRouter(routes);

export { router };
