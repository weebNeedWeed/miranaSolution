import React from "react";
import { Link, NavLink } from "react-router-dom";
import logo from "./../assets/logo.svg";

const Navbar = (): JSX.Element => {
	return (
		<nav className="w-full bg-whiteChocolate py-8 px-16 flex flex-row">
			<div className="flex flex-row items-center justify-start">
				<div className="mr-16">
					<Link to="/">
						<img src={logo} alt="logo" className="h-[20px]" />
					</Link>
				</div>

				<div className="flex flex-row items-center">
					{Array.from(new Array(3)).map((elm, index) => (
						<p
							className="flex items-end font-sansPro mt-1 mr-10 text-lg text-deepKoamaru font-semibold"
							key={index}
						>
							<NavLink to="/">Home</NavLink>
						</p>
					))}
				</div>
			</div>

			<div className="grow flex flex-row justify-end items-center mt-1">
				<p className="mr-4 font-semibold text-deepKoamaru text-lg font-sansPro">
					<Link to="/">{"Đăng ký"}</Link>
				</p>
				<button className="outline-none rounded-md border-2 border-solid border-deepKoamaru p-2 text-lg font-semibold font-sansPro text-deepKoamaru bg-[rgba(var(--color-old-rose),0.6)]">
					{"Đăng nhập"}
				</button>
			</div>
		</nav>
	);
};

export { Navbar };
