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
import {PageNotFound} from "../../pages/PageNotFound";
import {InternalServerError} from "../../pages/InternalServerError";
import {GlobalChecking} from "../../containers/GlobalChecking";

const routes: RouteObject[] = [
    {
        errorElement: <InternalServerError/>,
        element: <GlobalChecking/>,
        children: [
            {
                element: <DefaultLayout/>,
                children: [
                    {
                        path: "/",
                        element: <Home/>,
                        index: true,
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
        ]
    },
    {
        path: "404",
        element: <PageNotFound/>
    },
    {
        path: "500",
        element: <InternalServerError/>
    },
    {
        path: "*",
        element: <Navigate to={"/404"}/>
    }
];

const router: Router = createBrowserRouter(routes);

export {router};
