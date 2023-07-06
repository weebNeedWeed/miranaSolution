import {MouseEvent, useEffect, useState} from "react";
import {Link, NavLink, useNavigate} from "react-router-dom";
import logo from "../assets/logo.svg";
import {CgClose, CgMenuRight} from "react-icons/cg";
import {clsx} from "clsx";
import {Genre} from "../helpers/models/catalog/books/Genre";
import {useQuery} from "react-query";
import {genreApiHelper} from "../helpers/apis/GenreApiHelper";
import {useAuthenticationContext} from "../contexts/AuthenticationContext";
import {BiUser} from "react-icons/bi";
import {useAccessToken} from "../helpers/hooks/useAccessToken";
import {useSystemContext} from "../contexts/SystemContext";
import {ToastVariant} from "./Toast";
import {Avatar} from "./Avatar";

type GenresBoxProps = {};
const GenresBox = (props: GenresBoxProps): JSX.Element => {
    const {isLoading, data, error} = useQuery("genres", () => genreApiHelper.getAll(), {
        staleTime: Infinity
    });

    if (isLoading || error || !data) {
        return <></>;
    }

    const genreTupleList: Genre[][] = [];
    const tupleItemLength = 2;
    data!.forEach((elm, index) => {
        if (index % tupleItemLength === 0) {
            genreTupleList.push([elm]);
        } else {
            const indexOfPreDefinedItem = Math.floor(index / tupleItemLength);
            genreTupleList[indexOfPreDefinedItem].push(elm);
        }
    });

    // Map the list of tuple into JSX
    const mappedGenreTupleList = genreTupleList.map((tuple, tIndex) => {
        return <div className="flex flex-row ml-[-10px]" key={tIndex}>
            {tuple.map((genre, gIndex) =>
                <Link to={""} className="w-[calc(calc(100%/2)-10px)] ml-[10px] text-base font-normal"
                      key={gIndex}>{genre.name}</Link>)}
        </div>;
    });

    return <div className="flex flex-col w-full p-3">
        {mappedGenreTupleList}
    </div>;
};

type MenuProps = {
    className?: string;
};
const Menu = ({className}: MenuProps): JSX.Element => {
    const pClass = clsx(
        "font-sansPro text-deepKoamaru font-semibold",
        className && className
    );

    return (
        <>
            <div className={pClass}>
                <NavLink to="/">Trang chủ</NavLink>
            </div>
            <div className={pClass}>
                <NavLink to="/books">Tìm kiếm</NavLink>
            </div>
            <div className={clsx(pClass, "group relative")}>
                <button>Thể loại</button>

                <div
                    className="shadow-md shadow-oldRose md:group-hover:block left-[50%] absolute hidden bg-oldRose top-full opacity-80 w-56 h-auto translate-x-[-50%] rounded-sm">
                    <GenresBox/>
                </div>
            </div>
        </>
    );
};

const Navbar = (): JSX.Element => {
    const [toggleMenu, setToggleMenu] = useState<boolean>(false);
    const {state: authenticationState, dispatch: authenticationDispatch} = useAuthenticationContext();
    const {state: systemState, dispatch: systemDispatch} = useSystemContext();
    const [openUserMenu, setOpenUserMenu] = useState<boolean>(false);
    const [accessToken, setAccessToken] = useAccessToken();
    const navigate = useNavigate();

    const isLoggedIn = typeof authenticationState.user.id !== "undefined";

    const handleLogout = () => {
        setAccessToken("");
        systemDispatch({
            type: "addToast", payload: {
                title: "Đăng xuất thành công",
                variant: ToastVariant.Success
            }
        });

        // Reload after 2 seconds
        setTimeout(() => {
            navigate(0);
        }, 2000);
    }

    const handleToggleMenu = (event: MouseEvent<HTMLButtonElement>) => {
        setToggleMenu((prev) => !prev);
    };

    useEffect(() => {
        if (toggleMenu) {
            document.body.style.overflowY = "hidden";
            return () => {
                document.body.style.overflowY = "scroll";
            };
        }
    }, [toggleMenu]);

    const renderMobileMenu = toggleMenu && (
        <>
            <div className="absolute z-10 top-0 left-full bg-[rgba(0,0,0,0.4)] w-full h-full animate-slideLeft"></div>
            <div
                className="absolute z-10 top-0 left-full h-full w-[min(300px,75vw)] bg-darkVanilla block shadow-2xl animate-slideLeft">
                <div className="w-full flex flex-col pt-28 px-8 items-end text-2xl">
                    {isLoggedIn &&
                        <Link to={"/user/profile"}
                              className="flex flex-col justify-end items-end cursor-pointer relative mb-4">
                            <Avatar className="w-14 h-14"/>

                            <span className="text-deepKoamaru text-xl">
                                Hello, <span
                                className="font-semibold">{authenticationState.user.firstName} {authenticationState.user.lastName}</span>
                            </span>
                        </Link>}

                    <Menu className="flex mb-4"/>

                    {isLoggedIn ? <div className="sm:hidden flex flex-col items-end">
                        <button
                            onClick={handleLogout}
                            className="text-red-600 font-semibold text-deepKoamaru text-2xl font-sansPro mb-8"
                        >
                            {"Đăng xuất"}
                        </button>
                    </div> : <div className="sm:hidden flex flex-col items-end">
                        <p className="font-semibold text-deepKoamaru text-2xl font-sansPro mb-8">
                            <Link to="/auth/register">{"Đăng ký"}</Link>
                        </p>
                        <Link
                            to="/auth/login"
                            className="outline-none rounded-md border-2 border-solid border-deepKoamaru p-2 text-2xl font-semibold font-sansPro text-deepKoamaru bg-[rgba(var(--color-old-rose),0.6)]"
                        >
                            {"Đăng nhập"}
                        </Link>
                    </div>}
                </div>
            </div>
        </>
    );

    return (
        <nav className="w-full bg-gradient py-8 lg:px-16 px-8 flex flex-row">
            <div className="flex flex-row items-center justify-start">
                <div className="mr-16 h-[48px] flex items-center">
                    <Link to="/">
                        <img src={logo} alt="logo" className="h-[20px]"/>
                    </Link>
                </div>

                <div className="hidden md:flex flex-row items-center">
                    <Menu className="mt-1 mr-10 text-lg flex items-center"/>
                </div>
            </div>

            <div className="grow hidden sm:flex flex-row justify-end items-center mt-1">
                {isLoggedIn ? <div className="flex flex-row justify-end items-center cursor-pointer relative"
                                   onClick={() => setOpenUserMenu(!openUserMenu)}>
                    <span className="text-deepKoamaru text-lg">
                        Hello, <span
                        className="font-semibold">{authenticationState.user.firstName} {authenticationState.user.lastName}</span>
                    </span>

                    <Avatar className="w-9 h-9"/>

                    {openUserMenu &&
                        <div
                            className="rounded z-10 absolute top-[110%] right-0 w-3/4 p-4 flex flex-col bg-oldRose opacity-80 justify-start items-start">
                            <Link className="w-full" to={"/user/profile"}>
                                Hồ sơ
                            </Link>
                            <button className="w-full text-left text-red-600 font-semibold"
                                    onClick={handleLogout}>
                                Đăng xuất
                            </button>
                        </div>}
                </div> : <>
                    <p className="mr-4 font-semibold text-deepKoamaru text-lg font-sansPro">
                        <Link to="/auth/register">{"Đăng ký"}</Link>
                    </p>
                    <Link
                        to="/auth/login"
                        className="outline-none rounded-md border-2 border-solid border-deepKoamaru p-2 text-lg font-semibold font-sansPro text-deepKoamaru bg-[rgba(var(--color-old-rose),0.6)]"
                    >
                        {"Đăng nhập"}
                    </Link>
                </>}
            </div>

            <div className="flex md:hidden grow sm:grow-0 ml-4 items-center justify-end mt-1">
                <button onClick={handleToggleMenu} className="text-2xl relative z-20">
                    {toggleMenu ? <CgClose/> : <CgMenuRight/>}
                </button>

                {renderMobileMenu}
            </div>
        </nav>
    );
};

export {Navbar};