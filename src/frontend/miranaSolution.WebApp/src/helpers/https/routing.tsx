import {Router} from "@remix-run/router";
import {createBrowserRouter, Navigate} from "react-router-dom";
import {RouteObject} from "react-router/dist/lib/context";
import {Home} from "../../pages";
import {AuthLayout, DefaultLayout, UserLayout} from "../../layouts";
import {Test} from "../../pages/Test";
import {BooksChapter, BooksIndex, BooksInfo} from "../../pages/books";
import React from "react";
import {UserBookmarks, UserCurrentlyReadings, UserPassword, UserProfile} from "../../pages/user";
import {Login, PasswordRecovery, RedeemToken, Register} from "../../pages/auth";

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
                                element: <BooksChapter/>
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
                        element: <UserPassword/>
                    },
                    {
                        path: "currently-readings",
                        element: <UserCurrentlyReadings/>
                    },
                    {
                        path: "bookmarks",
                        element: <UserBookmarks/>
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
            {
                path: "password-recovery",
                children: [
                    {
                        index: true,
                        element: <PasswordRecovery/>
                    },
                    {
                        path: "redeem-token",
                        element: <RedeemToken/>,
                    }
                ]
            },
        ],
    },
];

const router: Router = createBrowserRouter(routes);

export {router};
