import { useState, MouseEvent, useEffect } from "react";
import { Link, NavLink } from "react-router-dom";
import logo from "./../assets/logo.svg";
import { CgMenuRight, CgClose } from "react-icons/cg";
import { clsx } from "clsx";

type MenuProps = {
	className?: string;
};
const Menu = ({ className }: MenuProps): JSX.Element => {
	const pClass = clsx(
		"font-sansPro text-deepKoamaru font-semibold",
		className && className,
	);

	return (
		<>
			<p className={pClass}>
				<NavLink to="/">Trang chủ</NavLink>
			</p>
			<p className={pClass}>
				<NavLink to="/test">Tìm kiếm</NavLink>
			</p>
			<p className={pClass}>
				<NavLink to="/">Thể loại</NavLink>
			</p>
		</>
	);
};

const Navbar = (): JSX.Element => {
	const [toggleMenu, setToggleMenu] = useState<boolean>(false);

	const handleToggleMenu = (event: MouseEvent<HTMLButtonElement>) => {
		setToggleMenu((prev) => !prev);
	};

	useEffect(() => {
		if (toggleMenu === true) {
			document.body.style.overflow = "hidden";
			return () => {
				document.body.style.overflow = "scroll";
			};
		}
	}, [toggleMenu]);

	const renderMobileMenu = toggleMenu && (
		<>
			<div className="absolute z-0 top-0 left-full bg-[rgba(0,0,0,0.4)] w-full h-full animate-slideLeft"></div>
			<div className="absolute z-10 top-0 left-full h-full w-[min(300px,75vw)] bg-darkVanilla block shadow-2xl animate-slideLeft">
				<div className="w-full flex flex-col pt-28 px-8 items-end text-2xl">
					<Menu className="flex mb-4" />
					<div className="sm:hidden flex flex-col items-end">
						<p className="font-semibold text-deepKoamaru text-2xl font-sansPro mb-8">
							<Link to="/">{"Đăng ký"}</Link>
						</p>
						<button className="outline-none rounded-md border-2 border-solid border-deepKoamaru p-2 text-2xl font-semibold font-sansPro text-deepKoamaru bg-[rgba(var(--color-old-rose),0.6)]">
							{"Đăng nhập"}
						</button>
					</div>
				</div>
			</div>
		</>
	);

	return (
		<nav className="w-full bg-gradient py-8 lg:px-16 px-8 flex flex-row">
			<div className="flex flex-row items-center justify-start">
				<div className="mr-16 h-[48px] flex items-center">
					<Link to="/">
						<img src={logo} alt="logo" className="h-[20px]" />
					</Link>
				</div>

				<div className="hidden md:flex flex-row items-center">
					<Menu className="mt-1 mr-10 text-lg flex items-center" />
				</div>
			</div>

			<div className="grow hidden sm:flex flex-row justify-end items-center mt-1">
				<p className="mr-4 font-semibold text-deepKoamaru text-lg font-sansPro">
					<Link to="/">{"Đăng ký"}</Link>
				</p>
				<button className="outline-none rounded-md border-2 border-solid border-deepKoamaru p-2 text-lg font-semibold font-sansPro text-deepKoamaru bg-[rgba(var(--color-old-rose),0.6)]">
					{"Đăng nhập"}
				</button>
			</div>

			<div className="flex md:hidden grow sm:grow-0 ml-4 items-center justify-end mt-1">
				<button onClick={handleToggleMenu} className="text-2xl relative z-20">
					{toggleMenu ? <CgClose /> : <CgMenuRight />}
				</button>

				{renderMobileMenu}
			</div>
		</nav>
	);
};

export { Navbar };
