import { Outlet } from "react-router-dom";

const AuthLayout = (): JSX.Element => {
  return (
    <div className="flex w-full justify-center">
      <div className="my-20 w-[min(400px,75vw)]">
        <Outlet />
      </div>
    </div>
  );
};

export { AuthLayout };
