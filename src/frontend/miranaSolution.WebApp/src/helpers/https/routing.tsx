import {Router} from "@remix-run/router";
import {Navigate, createBrowserRouter} from "react-router-dom";
import {RouteObject} from "react-router/dist/lib/context";

import {Home} from "../../pages";
import {DefaultLayout, UserLayout} from "../../layouts";
import {Test} from "../../pages/Test";
import {AuthLayout} from "../../layouts";
import {BooksChapter, BooksIndex, BooksInfo} from "../../pages/books";
import {booksInfoLoader} from "./loaders/booksLoader";
import React from "react";
import {UserProfile} from "../../pages/user";
import {Login, Register} from "../../pages/auth";

const routes: RouteObject[] = [
    {
        element: <DefaultLayout/>,
        children: [
            {
                path: "/",
                element: <Home/>,
                index: true,
            },
            {
                path: "/test",
                element: <Test/>,
            },
            {
                path: "/books",
                children: [
                    {
                        index: true,
                        element: <BooksIndex/>,
                    },
                    {
                        path: ":slug",
                        children: [
                            {
                                index: true,
                                element: <BooksInfo/>,
                            },
                            {
                                path: "chapters/:index",
                                element: <BooksChapter/>,
                                loader: booksInfoLoader
                            },
                        ]
                    },
                ],
            },
            {
                path: "/user",
                element: <UserLayout/>,
                children: [
                    {
                        index: true,
                        element: <Navigate to={"/user/profile"}/>
                    },
                    {
                        path: "profile",
                        element: <UserProfile/>
                    },
                    {
                        path: "password",
                        element: <p>meomeo</p>
                    }
                ]
            }
        ],
    },
    {
        path: "/auth",
        element: <AuthLayout/>,
        children: [
            {
                element: <Navigate to="/auth/login"/>,
                index: true,
            },
            {
                path: "login",
                element: <Login/>,
            },
            {
                path: "register",
                element: <Register/>,
            },
        ],
    },
];

const router: Router = createBrowserRouter(routes);

export {router};
