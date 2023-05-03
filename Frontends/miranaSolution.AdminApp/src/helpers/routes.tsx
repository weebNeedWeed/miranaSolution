import { createBrowserRouter, Navigate } from "react-router-dom";
import { DefaultLayout } from "../layouts";
import { LoginPage, DashboardPage, UsersPage } from "../pages";

const router = createBrowserRouter([
  {
    index: true,
    path: "/",
    element: <Navigate to={"/dashboard"}/>,
  },
  {
    path: "/dashboard",
    element: <DefaultLayout/>,
    children: [
      {
        index: true,
        element: <DashboardPage/>
      },
      {
        path: "users",
        element: <UsersPage/>
      }
    ]
  },
  {
    path: "/login",
    element: <LoginPage/>,
  },
]);

export { router };
