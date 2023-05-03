import { Outlet } from "react-router-dom";
import { NavBar } from "../components";

const DefaultLayout = (): JSX.Element => {
  return <>
    <header>
      <NavBar/>
    </header>
  </>;
};

export { DefaultLayout };
