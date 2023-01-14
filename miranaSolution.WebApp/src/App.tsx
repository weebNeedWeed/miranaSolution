import React from "react";
import { RouterProvider } from "react-router-dom";
import { router } from "./router";
import "./app.css";

const App = (): JSX.Element => {
	return <RouterProvider router={router} />;
};

export { App };
