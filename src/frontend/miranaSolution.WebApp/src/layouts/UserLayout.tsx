import {Section} from "../components";
import {FaLock, FaUserAlt} from "react-icons/fa";
import clsx from "clsx";
import {NavLink, Outlet, useNavigate} from "react-router-dom";
import {useAuthenticationContext} from "../contexts/AuthenticationContext";
import {useEffect} from "react";

type SideNavButtonProps = {
    title: string,
    icon: React.ReactNode,
    to: string
};
const SideNavButton = (props: SideNavButtonProps): JSX.Element => {
    const {title, icon, to} = props;
    const activeClasses = "bg-[rgba(255,255,255,0.6)]";

    return <NavLink
        to={to}
        className={({isActive}) =>
            clsx(isActive && activeClasses, "cursor-pointer p-3 sm:px-2 sm:py-1 rounded w-full flex flex-row justify-start items-center gap-x-2 text-lg font-semibold")
        }>
        <span className="text-base">{icon}</span>
        <span className="hidden sm:inline-block">{title}</span>
    </NavLink>;
};

const UserLayout = (): JSX.Element => {
    const authContext = useAuthenticationContext();
    const navigate = useNavigate();

    useEffect(() => {
        if (!authContext.state.isLoggedIn) {
            navigate("/");
        }
    }, [authContext.state.isLoggedIn]);

    return <Section className="text-deepKoamaru">
        <div className="flex w-full flex-row items-stretch">
            <div className="sm:w-1/4 bg-oldRose p-3 md:p-6 rounded shrink-0 shadow-deepKoamaru shadow-sm">
                <div className="flex flex-col justify-start items-start gap-y-3">
                    <SideNavButton to="/user/profile" icon={<FaUserAlt/>} title={"Tài khoản"}/>

                    <SideNavButton to="/user/password" icon={<FaLock/>} title={"Mật khẩu"}/>
                </div>
            </div>

            <div className="bg-[rgba(255,255,255,0.8)] grow w-full p-3 md:p-6 rounded-r-md min-h-[850px]">
                <Outlet/>
            </div>
        </div>
    </Section>
}

export {UserLayout};