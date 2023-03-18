import { useId } from "react";
import { AiOutlineUser, AiOutlineLock } from "react-icons/ai";
import { Link } from "react-router-dom";

const Register = (): JSX.Element => {
  const userNameId = useId();
  const passwordId = useId();
  const confirmPasswordId = useId();
  const firstNameId = useId();
  const lastNameId = useId();
  const emailId = useId();

  return (
    <form className="bg-white w-full p-8 sm:p-12 rounded-md shadow-md shadow-slate-500">
      <h3 className="font-bold text-3xl text-center text-deepKoamaru">
        Register
      </h3>

      <div className="flex flex-col items-start justify-start mt-8">
        <label
          htmlFor={lastNameId}
          className="font-semibold text-sm text-slate-600"
        >
          Họ
        </label>
        <div className="flex flex-row w-full justify-start items-center py-2 px-1 border-slate-300 border-b-2 border-solid gap-x-2 text-sm">
          <AiOutlineUser className="text-slate-400" />
          <input
            type="text"
            id={lastNameId}
            required
            className="text-slate-400 grow focus:border-0 outline-none"
            placeholder="Nhập họ của bạn"
          />
        </div>
      </div>

      <div className="flex flex-col items-start justify-start mt-4">
        <label
          htmlFor={firstNameId}
          className="font-semibold text-sm text-slate-600"
        >
          Tên
        </label>
        <div className="flex flex-row w-full justify-start items-center py-2 px-1 border-slate-300 border-b-2 border-solid gap-x-2 text-sm">
          <AiOutlineUser className="text-slate-400" />
          <input
            type="text"
            required
            id={firstNameId}
            className="text-slate-400 grow focus:border-0 outline-none"
            placeholder="Nhập tên của bạn"
          />
        </div>
      </div>

      <div className="flex flex-col items-start justify-start mt-4">
        <label
          htmlFor={emailId}
          className="font-semibold text-sm text-slate-600"
        >
          Email
        </label>
        <div className="flex flex-row w-full justify-start items-center py-2 px-1 border-slate-300 border-b-2 border-solid gap-x-2 text-sm">
          <AiOutlineUser className="text-slate-400" />
          <input
            type="email"
            required
            id={emailId}
            className="text-slate-400 grow focus:border-0 outline-none"
            placeholder="Nhập email"
          />
        </div>
      </div>

      <div className="flex flex-col items-start justify-start mt-4">
        <label
          htmlFor={userNameId}
          className="font-semibold text-sm text-slate-600"
        >
          Tài khoản
        </label>
        <div className="flex flex-row w-full justify-start items-center py-2 px-1 border-slate-300 border-b-2 border-solid gap-x-2 text-sm">
          <AiOutlineUser className="text-slate-400" />
          <input
            type="text"
            id={userNameId}
            required
            className="text-slate-400 grow focus:border-0 outline-none"
            placeholder="Nhập tài khoản"
          />
        </div>
      </div>
      <div className="flex flex-col items-start justify-start mt-4">
        <label
          htmlFor={passwordId}
          className="font-semibold text-sm text-slate-600"
        >
          Mật khẩu
        </label>
        <div className="flex flex-row w-full justify-start items-center py-2 px-1 border-slate-300 border-b-2 border-solid gap-x-2 text-sm">
          <AiOutlineLock className="text-slate-400" />
          <input
            id={passwordId}
            type="password"
            className="text-slate-400 grow focus:border-0 outline-none"
            placeholder="Nhập mật khẩu"
          />
        </div>
      </div>

      <div className="flex flex-col items-start justify-start mt-4">
        <label
          htmlFor={confirmPasswordId}
          className="font-semibold text-sm text-slate-600"
        >
          Nhập lại mật khẩu
        </label>
        <div className="flex flex-row w-full justify-start items-center py-2 px-1 border-slate-300 border-b-2 border-solid gap-x-2 text-sm">
          <AiOutlineLock className="text-slate-400" />
          <input
            id={confirmPasswordId}
            type="password"
            required
            className="text-slate-400 grow focus:border-0 outline-none"
            placeholder="Nhập lại mật khẩu"
          />
        </div>
      </div>

      <button
        type="submit"
        className="w-full py-3 bg-oldRose text-deepKoamaru uppercase font-bold mt-8"
      >
        Đăng ký
      </button>

      <p className="text-center text-sm text-slate-600 mt-1">
        Hoặc{" "}
        <Link to="/auth/login" className="underline">
          đăng nhập
        </Link>
        .
      </p>
    </form>
  );
};

export { Register };
