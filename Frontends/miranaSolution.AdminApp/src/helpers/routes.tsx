import { createBrowserRouter, Navigate } from "react-router-dom";
import { DefaultLayout } from "../layouts";
import { LoginPage, DashboardPage, UsersPage } from "../pages";
import { BooksIndexPage } from "../pages/books/BooksIndexPage";
import { BooksCreatePage } from "../pages/books/BooksCreatePage";

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
      },
      {
        path: "books",
        children: [
          {
            index: true,
            element: <BooksIndexPage/>
          },
          {
            path: "create",
            element: <BooksCreatePage/>
          }
        ]
      }
    ]
  },
  {
    path: "/login",
    element: <LoginPage/>,
  },
]);

export { router };
