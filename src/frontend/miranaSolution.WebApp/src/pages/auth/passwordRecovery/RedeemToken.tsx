import {Link, useNavigate, useSearchParams} from "react-router-dom";
import React, {useEffect, useId, useState} from "react";
import {IoIosArrowBack} from "react-icons/io";
import {AiOutlineLock} from "react-icons/ai";
import {authApiHelper} from "../../../helpers/apis/AuthApiHelper";
import {ToastVariant} from "../../../components/Toast";
import {useSystemContext} from "../../../contexts/SystemContext";

const RedeemToken = (): JSX.Element => {
    const [searchParams] = useSearchParams();
    const navigate = useNavigate();
    const token = searchParams.get("token");
    const email = searchParams.get("email");

    const [passwordErrors, setPasswordErrors] = useState<string[]>([]);
    const [confirmPasswordErrors, setConfirmPasswordErrors] = useState<string[]>([]);

    const [password, setPassword] = useState("");
    const [confirmPassword, setConfirmPassword] = useState("");

    const passwordId = useId();
    const confirmPasswordId = useId();

    const systemContext = useSystemContext();

    useEffect(() => {
        if (token == null || email == null) {
            navigate("/auth/password-recovery");
        }
    }, [token, email]);

    const isValidForm = (): boolean => {
        if (password.trim().length === 0) {
            setPasswordErrors(["Mật khẩu không được trống."]);
            return false;
        }

        if (confirmPassword.trim().length === 0) {
            setConfirmPasswordErrors(["Vui lòng nhập lại mật khẩu."]);
            return false;
        }

        return true;
    }

    const handleSubmit = async (event: React.FormEvent<HTMLFormElement>) => {
        event.preventDefault();

        if (!isValidForm()) {
            return;
        }

        try {
            console.log({
                token: token!,
                email: email!,
                newPassword: password,
                newPasswordConfirmation: confirmPassword
            });

            await authApiHelper.redeemToken({
                token: token!,
                email: email!,
                newPassword: password,
                newPasswordConfirmation: confirmPassword
            });

            systemContext.dispatch({
                type: "addToast", payload: {
                    title: "Tạo mật khẩu mới thành công, vui lòng đăng nhập lại.",
                    variant: ToastVariant.Success
                }
            });

            setTimeout(() => {
                navigate("/auth/login");
            }, 3000);
        } catch (err: any) {
            systemContext.dispatch({
                type: "addToast", payload: {
                    title: err.message,
                    variant: ToastVariant.Error
                }
            });
        }
    };

    const handlePasswordChange = (event: React.ChangeEvent<HTMLInputElement>) => {
        const _password = event.target.value;
        if (_password.trim().length === 0) {
            setPasswordErrors(["Mật khẩu không được trống."]);
        } else {
            setPasswordErrors([]);
        }

        setPassword(_password);
    }

    const handleConfirmPasswordChange = (event: React.ChangeEvent<HTMLInputElement>) => {
        const _confirmPassword = event.target.value;

        if (_confirmPassword.trim().length === 0) {
            setConfirmPasswordErrors(["Vui lòng nhập lại mật khẩu."]);
        } else {
            setConfirmPasswordErrors([]);
        }

        setConfirmPassword(_confirmPassword);
    }

    return <form
        onSubmit={handleSubmit}
        className="relative bg-white w-full p-8 sm:p-12 rounded-md shadow-md shadow-slate-500"
    >
        <Link to={"/"}
              className="flex flex-row items-center gap-x-1 absolute top-3 left-3 text-sm font-normal text-slate-600">
            <IoIosArrowBack/>
            Về trang chủ
        </Link>

        <div className="flex flex-col items-start justify-start mt-4">
            <label
                htmlFor={passwordId}
                className="font-semibold text-sm text-slate-600"
            >
                Mật khẩu
            </label>
            <div
                className="flex flex-row w-full justify-start items-center py-2 px-1 border-slate-300 border-b-2 border-solid gap-x-2 text-sm">
                <AiOutlineLock className="text-slate-400"/>
                <input
                    value={password}
                    onChange={handlePasswordChange}
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
            <div
                className="flex flex-row w-full justify-start items-center py-2 px-1 border-slate-300 border-b-2 border-solid gap-x-2 text-sm">
                <AiOutlineLock className="text-slate-400"/>
                <input
                    value={confirmPassword}
                    onChange={handleConfirmPasswordChange}
                    id={confirmPasswordId}
                    type="password"
                    className="text-slate-400 grow focus:border-0 outline-none"
                    placeholder="Nhập lại mật khẩu"
                />
            </div>

        </div>

        <button
            type="submit"
            className="w-full py-3 bg-oldRose text-deepKoamaru uppercase font-bold mt-6"
        >
            Tạo mật khẩu
        </button>

        <p className="text-center text-sm text-slate-600 mt-1">
            Về{" "}
            <Link to="/auth/login" className="underline">
                đăng nhập
            </Link>
            .
        </p>
    </form>;
}

export {RedeemToken};