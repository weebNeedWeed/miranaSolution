import { Outlet } from "react-router-dom";
import { LoadingScreen, ToastContainer } from "../containers";

const AuthLayout = (): JSX.Element => {
  return (
    <div className="flex w-full justify-center min-h-[100vh]">
      <div className="my-20 w-[min(400px,80vw)]">
        <Outlet/>
      </div>

      <LoadingScreen/>

      <ToastContainer/>
    </div>
  );
};

export { AuthLayout };
