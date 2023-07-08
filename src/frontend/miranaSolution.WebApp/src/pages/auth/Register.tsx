import {useId, useState} from "react";
import {AiOutlineUser, AiOutlineLock} from "react-icons/ai";
import {Link, useNavigate} from "react-router-dom";
import {userApiHelper, ValidateFailedMessage} from "../../helpers/apis/UserApiHelper";
import {UserRegisterRequest} from "../../helpers/models/auth/UserRegisterRequest";
import {User} from "../../helpers/models/auth/User";
import {useSystemContext} from "../../contexts/SystemContext";

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

    const [ableToSubmit, setAbleToSubmit] = useState(false);

    const handleSubmit = async (event: React.FormEvent<HTMLFormElement>) => {
        event.preventDefault();

        if (!ableToSubmit) {
            validateName(setLastNameErrors, "Last name")(lastName);
            validateName(setFirstNameErrors, "First name")(firstName);
            validateEmail(email);
            validateUserName(userName);
            validatePassword(password);
            validateConfirmPassword(confirmPassword);
            return;
        }

        const userRegisterRequest: UserRegisterRequest = {
            FirstName: firstName,
            LastName: lastName,
            Email: email,
            UserName: userName,
            Password: password
        };

        const res = await userApiHelper.register(userRegisterRequest);

        if (res === null) {
            // TODO: Display message in the alert box
            setFirstNameErrors(["Unknown errors. Please try again later!"]);
            return;
        }


        function isUser(obj: any): obj is User {
            return Object.keys(obj).some(x => typeof obj[x] !== "object");
        }

        if (!isUser(res)) {
            for (let key in res) {
                switch (key) {
                    case "Email":
                        setEmailErrors(res[key]);
                        break;
                    case "FirstName":
                        setFirstNameErrors(res[key]);
                        break;
                    case "LastName":
                        setLastNameErrors(res[key]);
                        break;
                    case "Password":
                        setPasswordErrors(res[key]);
                        break;
                    case "UserName":
                        setUserNameErrors(res[key]);
                        break;
                    default:
                        break;
                }
            }

            return;
        }

        dispatch({
            type: "addToast", payload: {
                title: "Đăng ký thành công, vui lòng đăng nhập"
            }
        });

        setTimeout(() => {
            navigate("/auth/login");
        }, 3000);
    };

    const handleInputChange = (
        setValueFn: React.Dispatch<string>,
        validateFn: (data: string) => void) => {
        return (event: React.ChangeEvent<HTMLInputElement>) => {
            const value = event.target.value;
            setValueFn(value);
            validateFn(value);
        };
    };

    const validateName = (setErrorsFn: React.Dispatch<string[]>, fieldName: string) => {
        return (name: string) => {
            const errors: string[] = [];

            setAbleToSubmit(true);

            if (name.trim().length === 0) {
                setAbleToSubmit(false);
                errors.push(fieldName + " is required");
            }

            setErrorsFn(errors);
        };
    };

    const validateEmail = (email: string) => {
        const errors: string[] = [];

        setAbleToSubmit(true);

        if (email.trim().length === 0) {
            setAbleToSubmit(false);
            errors.push("Email is required");
        }

        const pattern = new RegExp(/^[a-z0-9._%+-]+@[a-z0-9.-]+\.[a-z]{2,4}$/);

        if (!pattern.test(email)) {
            setAbleToSubmit(false);
            errors.push("Input must be an email.");
        }

        setEmailErrors(errors);
    };

    const validateUserName = (userName: string) => {
        const errors: string[] = [];

        setAbleToSubmit(true);

        if (userName.trim().length === 0) {
            setAbleToSubmit(false);
            errors.push("User name is required");
        }

        if (userName.length < 8) {
            setAbleToSubmit(false);
            errors.push("User name must be at least 8 characters");
        }

        setUserNameErrors(errors);
    };

    const validatePassword = (password: string) => {
        var errors: Array<string> = [];

        setAbleToSubmit(true);

        const regex = new RegExp(/^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)[a-zA-Z\d]{8,}$/);

        if (!password.trim().match(regex)) {
            setAbleToSubmit(false);
            errors.push(
                "Password must contain minimum eight characters, at least one uppercase letter, one lowercase letter and one number!"
            );
        }

        setPasswordErrors(errors);
    };

    const validateConfirmPassword = (confirmPassword: string) => {
        var errors: Array<string> = [];

        setAbleToSubmit(true);

        if (password !== confirmPassword) {
            setAbleToSubmit(false);
            errors.push(
                "Password is not equal"
            );
        }

        setConfirmPasswordErrors(errors);
    };

    return (
        <form method="POST" onSubmit={handleSubmit}
              className="bg-white w-full p-8 sm:p-12 rounded-md shadow-md shadow-slate-500">
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
                        onChange={handleInputChange(setLastName, validateName(setLastNameErrors, "Last name"))}
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
                        onChange={handleInputChange(setFirstName, validateName(setFirstNameErrors, "First name"))}
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
                        onChange={handleInputChange(setEmail, validateEmail)}
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
                        onChange={handleInputChange(setUserName, validateUserName)}
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
                        onChange={handleInputChange(setPassword, validatePassword)}
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
                        onChange={handleInputChange(setConfirmPassword, validateConfirmPassword)}
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
