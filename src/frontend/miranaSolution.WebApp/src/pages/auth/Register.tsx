import React, {useId, useState} from "react";
import {AiOutlineLock, AiOutlineUser} from "react-icons/ai";
import {Link, useNavigate} from "react-router-dom";
import {useSystemContext} from "../../contexts/SystemContext";
import {ToastVariant} from "../../components/Toast";
import {RegisterUserRequest} from "../../helpers/models/catalog/user/RegisterUserRequest";
import {authApiHelper} from "../../helpers/apis/AuthApiHelper";
import {IoIosArrowBack} from "react-icons/io";
import {Helmet} from "react-helmet";

const Register = (): JSX.Element => {
    const navigate = useNavigate();

    const {state, dispatch} = useSystemContext();

    const userNameId = useId();
    const passwordId = useId();
    const confirmPasswordId = useId();
    const firstNameId = useId();
    const lastNameId = useId();
    const emailId = useId();

    const [lastName, setLastName] = useState("");
    const [firstName, setFirstName] = useState("");
    const [email, setEmail] = useState("");
    const [userName, setUserName] = useState("");
    const [password, setPassword] = useState("");
    const [confirmPassword, setConfirmPassword] = useState("");

    const [lastNameErrors, setLastNameErrors] = useState<string[]>([]);
    const [firstNameErrors, setFirstNameErrors] = useState<string[]>([]);
    const [emailErrors, setEmailErrors] = useState<string[]>([]);
    const [userNameErrors, setUserNameErrors] = useState<string[]>([]);
    const [passwordErrors, setPasswordErrors] = useState<string[]>([]);
    const [confirmPasswordErrors, setConfirmPasswordErrors] = useState<string[]>([]);

    const isValidForm = React.useCallback((): boolean => {

        if (lastName.trim().length === 0) {
            setLastNameErrors(["Họ không được trống."]);
            return false;
        }

        if (firstName.trim().length === 0) {
            setFirstNameErrors(["Tên không được trống."]);
            return false;
        }

        if (email.trim().length === 0) {
            setEmailErrors(["Email không được trống."]);
            return false;
        }

        if (userName.trim().length === 0) {
            setUserNameErrors(["Tài khoản không được trống."]);
            return false;
        }

        if (password.trim().length === 0) {
            setPasswordErrors(["Mật khẩu không được trống."]);
            return false;
        }

        if (confirmPassword.trim().length === 0) {
            setConfirmPasswordErrors(["Vui lòng nhập lại mật khẩu."]);
            return false;
        }

        const pattern = new RegExp(/^[a-z0-9._%+-]+@[a-z0-9.-]+\.[a-z]{2,4}$/);

        if (!pattern.test(email)) {
            setEmailErrors(["Phải là email."]);
            return false;
        }

        return true;
    }, [lastName, firstName, email, userName, password, confirmPassword]);

    const handleSubmit = async (event: React.FormEvent<HTMLFormElement>) => {
        event.preventDefault();

        if (!isValidForm()) {
            return;
        }

        const registerUserRequest: RegisterUserRequest = {
            firstName: firstName,
            lastName: lastName,
            email: email,
            userName: userName,
            password: password,
            passwordConfirmation: confirmPassword
        };

        try {
            const res = await authApiHelper.register(registerUserRequest);

            dispatch({
                type: "addToast", payload: {
                    title: "Đăng ký thành công, vui lòng đăng nhập!"
                }
            });

            setTimeout(() => {
                navigate("/auth/login");
            }, 3000);
        } catch (err: any) {
            dispatch({
                type: "addToast", payload: {
                    title: err.message,
                    variant: ToastVariant.Error
                }
            });
        }
    };

    const handleLastNameChange = (event: React.ChangeEvent<HTMLInputElement>) => {
        const _lastName = event.target.value;

        if (_lastName.trim().length === 0) {
            setLastNameErrors(["Họ không được trống."]);
        } else {
            setLastNameErrors([]);
        }

        setLastName(_lastName);
    }

    const handleFirstNameChange = (event: React.ChangeEvent<HTMLInputElement>) => {
        const _firstName = event.target.value;
        if (_firstName.trim().length === 0) {
            setFirstNameErrors(["Tên không được trống."]);
        } else {
            setFirstNameErrors([]);
        }

        setFirstName(_firstName);
    }

    const handleEmailChange = (event: React.ChangeEvent<HTMLInputElement>) => {
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

    const handleUserNameChange = (event: React.ChangeEvent<HTMLInputElement>) => {
        const _userName = event.target.value;
        if (_userName.trim().length === 0) {
            setUserNameErrors(["Tài khoản không được trống."]);
        } else {
            setUserNameErrors([]);
        }


        setUserName(_userName);
    }

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

    return (
        <form method="POST" onSubmit={handleSubmit}
              className="relative bg-white w-full p-8 sm:p-12 rounded-md shadow-md shadow-slate-500">
            <Helmet>
                <title>Register | Mirana Readers</title>
            </Helmet>

            <Link to={"/"}
                  className="flex flex-row items-center gap-x-1 absolute top-3 left-3 text-sm font-normal text-slate-600">
                <IoIosArrowBack/>
                Về trang chủ
            </Link>

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
                <div
                    className="flex flex-row w-full justify-start items-center py-2 px-1 border-slate-300 border-b-2 border-solid gap-x-2 text-sm">
                    <AiOutlineUser className="text-slate-400"/>
                    <input
                        value={lastName}
                        onChange={handleLastNameChange}
                        type="text"
                        id={lastNameId}
                        className="text-slate-400 grow focus:border-0 outline-none"
                        placeholder="Nhập họ của bạn"
                    />
                </div>
                {lastNameErrors.length > 0 && (
                    <ul className="text-red-500 text-sm list-disc ml-4">
                        {lastNameErrors.map((elm, index) => (
                            <li key={index}>{elm}</li>
                        ))}
                    </ul>
                )}
            </div>

            <div className="flex flex-col items-start justify-start mt-4">
                <label
                    htmlFor={firstNameId}
                    className="font-semibold text-sm text-slate-600"
                >
                    Tên
                </label>
                <div
                    className="flex flex-row w-full justify-start items-center py-2 px-1 border-slate-300 border-b-2 border-solid gap-x-2 text-sm">
                    <AiOutlineUser className="text-slate-400"/>
                    <input
                        type="text"
                        value={firstName}
                        onChange={handleFirstNameChange}
                        id={firstNameId}
                        className="text-slate-400 grow focus:border-0 outline-none"
                        placeholder="Nhập tên của bạn"
                    />
                </div>
                {firstNameErrors.length > 0 && (
                    <ul className="text-red-500 text-sm list-disc ml-4">
                        {firstNameErrors.map((elm, index) => (
                            <li key={index}>{elm}</li>
                        ))}
                    </ul>
                )}
            </div>

            <div className="flex flex-col items-start justify-start mt-4">
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
                        value={email}
                        onChange={handleEmailChange}
                        id={emailId}
                        className="text-slate-400 grow focus:border-0 outline-none"
                        placeholder="Nhập email"
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

            <div className="flex flex-col items-start justify-start mt-4">
                <label
                    htmlFor={userNameId}
                    className="font-semibold text-sm text-slate-600"
                >
                    Tài khoản
                </label>
                <div
                    className="flex flex-row w-full justify-start items-center py-2 px-1 border-slate-300 border-b-2 border-solid gap-x-2 text-sm">
                    <AiOutlineUser className="text-slate-400"/>
                    <input
                        type="text"
                        id={userNameId}
                        value={userName}
                        onChange={handleUserNameChange}
                        className="text-slate-400 grow focus:border-0 outline-none"
                        placeholder="Nhập tài khoản"
                    />
                </div>
                {userNameErrors.length > 0 && (
                    <ul className="text-red-500 text-sm list-disc ml-4">
                        {userNameErrors.map((elm, index) => (
                            <li key={index}>{elm}</li>
                        ))}
                    </ul>
                )}
            </div>

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
                        id={passwordId}
                        value={password}
                        onChange={handlePasswordChange}
                        type="password"
                        className="text-slate-400 grow focus:border-0 outline-none"
                        placeholder="Nhập mật khẩu"
                    />
                </div>

                {passwordErrors.length > 0 && (
                    <ul className="text-red-500 text-sm list-disc ml-4">
                        {passwordErrors.map((elm, index) => (
                            <li key={index}>{elm}</li>
                        ))}
                    </ul>
                )}
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
                        id={confirmPasswordId}
                        type="password"
                        value={confirmPassword}
                        onChange={handleConfirmPasswordChange}
                        className="text-slate-400 grow focus:border-0 outline-none"
                        placeholder="Nhập lại mật khẩu"
                    />
                </div>

                {confirmPasswordErrors.length > 0 && (
                    <ul className="text-red-500 text-sm list-disc ml-4">
                        {confirmPasswordErrors.map((elm, index) => (
                            <li key={index}>{elm}</li>
                        ))}
                    </ul>
                )}
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

export {Register};
