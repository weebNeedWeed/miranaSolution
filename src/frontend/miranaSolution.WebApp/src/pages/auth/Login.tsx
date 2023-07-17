import {useId, useState} from "react";
import {AiOutlineLock, AiOutlineUser} from "react-icons/ai";
import {Link, useNavigate} from "react-router-dom";
import {userApiHelper} from "../../helpers/apis/UserApiHelper";
import {useSystemContext} from "../../contexts/SystemContext";
import {useAccessToken} from "../../helpers/hooks/useAccessToken";
import {ToastVariant} from "../../components/Toast";
import {AuthenticateUserResponse} from "../../helpers/models/auth/AuthenticateUserResponse";
import {ValidationFailedMessages} from "../../helpers/models/common/ValidationFailedMessages";

const Login = (): JSX.Element => {
    const userNameId = useId();
    const passwordId = useId();

    const [accessToken, setAccessToken] = useAccessToken();

    const navigate = useNavigate();
    const systemContext = useSystemContext();

    const [userName, setUserName] = useState<string>("");
    const [password, setPassword] = useState<string>("");
    const [userNameErrors, setUserNameErrors] = useState<Array<string>>([]);
    const [passwordErrors, setPasswordErrors] = useState<Array<string>>([]);
    const [ableToSubmit, setAbleToSubmit] = useState<boolean>(false);

    const validateUserName = (userName: string) => {
        var errors: Array<string> = [];

        setAbleToSubmit(true);

        if (userName.trim().length < 8) {
            errors.push("User name must contain at least 8 characters");
            setAbleToSubmit(false);
        }

        setUserNameErrors(errors);
    };

    const validatePassword = (password: string) => {
        let errors: Array<string> = [];

        setAbleToSubmit(true);
        const regex = new RegExp(/^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[@$!%*?&])[A-Za-z\d@$!%*?&]{8,}$/);

        if (!password.trim().match(regex)) {
            errors.push(
                "Password must contain Minimum eight characters, at least one uppercase letter, one lowercase letter, one number and one special character!"
            );
            setAbleToSubmit(false);
        }

        setPasswordErrors(errors);
    };

    const handleChangeUserName = (event: React.ChangeEvent<HTMLInputElement>) => {
        const _userName = event.target.value;
        setUserName(_userName);
        validateUserName(_userName);
    };

    const handleChangePassword = (event: React.ChangeEvent<HTMLInputElement>) => {
        const _password = event.target.value;
        setPassword(_password);
        validatePassword(_password);
    };

    const handleSubmit = async (event: React.FormEvent<unknown>) => {
        event.preventDefault();

        if (!ableToSubmit) {
            validateUserName(userName);
            validatePassword(password);
            return;
        }

        try {
            const response = await userApiHelper.authenticate({userName, password});
            if ("token" in response) {
                systemContext.dispatch({
                    type: "addToast", payload: {
                        title: "Đăng nhập thành công",
                        variant: ToastVariant.Success
                    }
                });

                // Save access token into the localStorage
                let authData = response as AuthenticateUserResponse;
                setAccessToken(authData.token);

                setTimeout(() => {
                    navigate("/");
                }, 3000);

                return;
            }

            const errors: any = response as ValidationFailedMessages<AuthenticateUserResponse>;
            for (let key in errors) {
                switch (key) {
                    case "UserName":
                        setUserNameErrors(errors[key]);
                        break;
                    case "Password":
                        setPasswordErrors(errors[key]);
                        break;
                    default:
                        break;
                }
            }
        } catch (error: any) {
            systemContext.dispatch({
                type: "addToast", payload: {
                    title: error.message,
                    variant: ToastVariant.Error
                }
            });
        }
    };

    return (
        <form
            onSubmit={handleSubmit}
            className="bg-white w-full p-8 sm:p-12 rounded-md shadow-md shadow-slate-500"
        >
            <h3 className="font-bold text-3xl text-center text-deepKoamaru">Login</h3>

            <div className="flex flex-col items-start justify-start mt-8">
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
                        className="text-slate-400 grow focus:border-0 outline-none"
                        placeholder="Nhập tài khoản"
                        value={userName}
                        onChange={handleChangeUserName}
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
                        type="password"
                        className="text-slate-400 grow focus:border-0 outline-none"
                        placeholder="Nhập mật khẩu"
                        value={password}
                        onChange={handleChangePassword}
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

            <p className="text-end">
                <Link to="/" className="text-sm text-slate-600">
                    Quên mật khẩu?
                </Link>
            </p>

            <button
                type="submit"
                className="w-full py-3 bg-oldRose text-deepKoamaru uppercase font-bold mt-6"
            >
                Đăng nhập
            </button>

            <p className="text-center text-sm text-slate-600 mt-1">
                Hoặc{" "}
                <Link to="/auth/register" className="underline">
                    đăng ký
                </Link>
                .
            </p>
        </form>
    );
};

export {Login};
