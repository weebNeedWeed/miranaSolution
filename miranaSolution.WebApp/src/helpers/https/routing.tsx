import { Router } from "@remix-run/router";
import { Navigate, createBrowserRouter } from "react-router-dom";
import { RouteObject } from "react-router/dist/lib/context";

import { Home, Login } from "../../pages";
import { DefaultLayout } from "../../layouts/DefaultLayout";
import { Test } from "../../pages/Test";
import { AuthLayout } from "../../layouts/AuthLayout";

const routes: RouteObject[] = [
  {
    element: <DefaultLayout />,
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
  {
    path: "/auth",
    element: <AuthLayout />,
    children: [
      {
        element: <Navigate to="/auth/login" />,
        index: true,
      },
      {
        path: "login",
        element: <Login />,
      },
    ],
  },
];

const router: Router = createBrowserRouter(routes);

export { router };
