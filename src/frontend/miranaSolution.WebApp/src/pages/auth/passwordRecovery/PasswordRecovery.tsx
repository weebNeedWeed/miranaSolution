import {Link} from "react-router-dom";
import {IoIosArrowBack} from "react-icons/io";
import {AiOutlineUser} from "react-icons/ai";
import React, {useEffect, useId, useState} from "react";
import {useSystemContext} from "../../../contexts/SystemContext";
import {ToastVariant} from "../../../components/Toast";
import {authApiHelper} from "../../../helpers/apis/AuthApiHelper";
import clsx from "clsx";

const PasswordRecovery = (): JSX.Element => {
    const emailId = useId();
    const [email, setEmail] = useState("");
    const [emailErrors, setEmailErrors] = useState<string[]>([]);
    const systemContext = useSystemContext();
    const [secondsLeft, setSecondsLeft] = useState(0);

    useEffect(() => {
        if (secondsLeft > 0) {
            const timeout = setTimeout(() => {
                setSecondsLeft((prev) => prev - 1);
            }, 1000);

            return () => {
                clearTimeout(timeout);
            }
        }
    }, [secondsLeft]);

    const isValidForm = () => {
        const pattern = new RegExp(/^[a-z0-9._%+-]+@[a-z0-9.-]+\.[a-z]{2,4}$/);
        if (email.trim().length === 0) {
            setEmailErrors(["Email không được trống."]);
            return false;
        }

        if (!pattern.test(email)) {
            setEmailErrors(["Phải là email."]);
            return false;
        }

        return true;
    }

    const handleSubmit = async (event: React.FormEvent<HTMLFormElement>) => {
        event.preventDefault();
        if (!isValidForm()) {
            return;
        }

        const currentHost = `${window.location.protocol}//${window.location.hostname}`;
        try {
            await authApiHelper.sendRecoveryEmail({
                email: email,
                callback: `${currentHost}:${window.location.port}${window.location.pathname}/redeem-token`
            });

            systemContext.dispatch({
                type: "addToast",
                payload: {
                    title: "Thành công! Vui lòng kiểm tra email của bạn.",
                    variant: ToastVariant.Success
                }
            });

            setSecondsLeft(60);
        } catch (error: any) {
            systemContext.dispatch({
                type: "addToast",
                payload: {
                    title: error.message,
                    variant: ToastVariant.Error
                }
            });
        }
    }

    const handleChangeEmail = (event: React.ChangeEvent<HTMLInputElement>) => {
        const _email = event.target.value;

        const pattern = new RegExp(/^[a-z0-9._%+-]+@[a-z0-9.-]+\.[a-z]{2,4}$/);
        if (_email.trim().length === 0) {
            setEmailErrors(["Email không được trống."]);
        } else if (!pattern.test(_email)) {
            setEmailErrors(["Phải là email."]);
        } else {
            setEmailErrors([]);
        }

        setEmail(_email);
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

        <h3 className="font-bold text-3xl text-center text-deepKoamaru">Password Recovery</h3>

        <div className="flex flex-col items-start justify-start mt-8">
            <label
                htmlFor={emailId}
                className="font-semibold text-sm text-slate-600"
            >
                Email
            </label>
            <div
                className="flex flex-row w-full justify-start items-center py-2 px-1 border-slate-300 border-b-2 border-solid gap-x-2 text-sm">
                <AiOutlineUser className="text-slate-400"/>
                <input
                    type="email"
                    id={emailId}
                    className="text-slate-400 grow focus:border-0 outline-none"
                    placeholder="Nhập email"
                    value={email}
                    onChange={handleChangeEmail}
                />
            </div>
            {emailErrors.length > 0 && (
                <ul className="text-red-500 text-sm list-disc ml-4">
                    {emailErrors.map((elm, index) => (
                        <li key={index}>{elm}</li>
                    ))}
                </ul>
            )}
        </div>

        <button
            type="submit"
            disabled={secondsLeft !== 0}
            className={clsx(secondsLeft !== 0 && "bg-[rgba(48,54,89,0.5)]", "w-full py-3 bg-oldRose text-deepKoamaru uppercase font-bold mt-6")}
        >
            Gửi liên kết{secondsLeft !== 0 && `(${secondsLeft})`}
        </button>

        <p className="text-center text-sm text-slate-600 mt-1">
            Về{" "}
            <Link to="/auth/login" className="underline">
                đăng nhập
            </Link>
            .
        </p>
    </form>
}

export {PasswordRecovery};