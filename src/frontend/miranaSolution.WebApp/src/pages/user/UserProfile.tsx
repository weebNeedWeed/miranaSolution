import {Avatar, Dialog, TextInput} from "../../components";
import {useAuthenticationContext} from "../../contexts/AuthenticationContext";
import React, {useEffect, useState} from "react";
import {BsPencil} from "react-icons/bs";
import {FaPager} from "react-icons/fa";
import {useBaseUrl} from "../../helpers/hooks/useBaseUrl";
import clsx from "clsx";
import {userApiHelper} from "../../helpers/apis/UserApiHelper";
import {useAccessToken} from "../../helpers/hooks/useAccessToken";
import {useSystemContext} from "../../contexts/SystemContext";
import {ToastVariant} from "../../components/Toast";
import {useNavigate} from "react-router-dom";

const InfoChangingForm = (): JSX.Element => {
    const [firstName, setFirstName] = useState<string>("");
    const [lastName, setLastName] = useState<string>("");
    const [email, setEmail] = useState<string>("");
    const [avatar, setAvatar] = useState<File>();

    const [ableToSubmit, setAbleToSubmit] = useState<boolean>(false);

    const baseUrl = useBaseUrl();
    const navigate = useNavigate();
    const [accessToken, setAccessToken] = useAccessToken();
    const {state: authenticationState, dispatch: authenticationDispatch} = useAuthenticationContext();
    const {state: systemState, dispatch: systemDispatch} = useSystemContext();

    useEffect(() => {
        const user = authenticationState.user;
        setFirstName(user.firstName);
        setLastName(user.lastName);
        setEmail(user.email);
    }, [authenticationState.user]);

    const handleSubmit = async (event: React.FormEvent<HTMLFormElement>) => {
        event.preventDefault();
        if (!ableToSubmit) {
            return;
        }

        try {
            const user: any = await userApiHelper.updateUserInformation(accessToken, {
                firstName,
                lastName,
                email,
                avatar
            });

            if (typeof user.userName !== "string") {
                systemDispatch({
                    type: "addToast", payload: {
                        title: "Có lỗi xảy ra! Vui lòng thử lại.",
                        variant: ToastVariant.Error
                    }
                });
            } else {
                systemDispatch({
                    type: "addToast", payload: {
                        title: "Cập nhật thành công.",
                        variant: ToastVariant.Success
                    }
                })

                // Reload after 2 secs
                setTimeout(() => {
                    navigate(0);
                }, 2000);
            }
        } catch (error: any) {
            systemDispatch({
                type: "addToast", payload: {
                    title: error.message,
                    variant: ToastVariant.Error
                }
            });
        }
    }

    const validateForm = (firstName: string, lastName: string, email: string) => {
        if (firstName.trim().length === 0) {
            setAbleToSubmit(false);
            return;
        }

        if (lastName.trim().length === 0) {
            setAbleToSubmit(false);
            return;
        }

        if (email.trim().length === 0) {
            setAbleToSubmit(false);
            return;
        }

        const pattern = new RegExp(/^[\w-.]+@([\w-]+\.)+[\w-]{2,4}$/);
        if (!pattern.test(email)) {
            setAbleToSubmit(false);
            return;
        }

        setAbleToSubmit(true);
    }

    const handleFileChange = (e: React.ChangeEvent<HTMLInputElement>) => {
        if (e.target.files) {
            setAvatar(e.target.files[0]);
            validateForm(firstName, lastName, email);
        }
    };

    const handleLastNameChange = (event: React.ChangeEvent<HTMLInputElement>) => {
        const name = event.target.value;
        validateForm(firstName, name, email);
        setLastName(name);
    };

    const handleFirstNameChange = (event: React.ChangeEvent<HTMLInputElement>) => {
        const name = event.target.value;
        validateForm(name, lastName, email);
        setFirstName(name);
    };

    const handleEmailChange = (event: React.ChangeEvent<HTMLInputElement>) => {
        const newemail = event.target.value;
        validateForm(firstName, lastName, newemail);
        setEmail(newemail);
    };


    return <form className="w-full" onSubmit={handleSubmit}>
        <div className="w-full flex-col flex">
            <label
                className="font-semibold text-sm text-slate-600 mb-1"
            >
                Ảnh đại diện
            </label>
            {authenticationState.user.avatar !== "" ?
                (typeof avatar !== "undefined" ?
                    <Avatar imageUrl={URL.createObjectURL(avatar)} className="w-16 h-16 mb-2"/> :
                    <Avatar imageUrl={baseUrl + authenticationState.user.avatar} className="w-16 h-16 mb-2"/>)
                : <Avatar className="w-16 h-16 mb-2"/>}
            <input type="file" onChange={handleFileChange}/>
        </div>

        <div className="w-full mt-4">
            <TextInput
                label={"Họ"}
                icon={<FaPager/>}
                placeholder={"Nhập họ"}
                value={lastName}
                onChange={handleLastNameChange}
            />
        </div>

        <div className="w-full mt-4">
            <TextInput
                label={"Tên"}
                icon={<FaPager/>}
                placeholder={"Nhập tên"}
                value={firstName}
                onChange={handleFirstNameChange}
            />
        </div>

        <div className="w-full mt-4">
            <TextInput
                label={"Email"}
                icon={<FaPager/>}
                placeholder={"Nhập email"}
                value={email}
                onChange={handleEmailChange}
            />
        </div>

        <button
            disabled={!ableToSubmit}
            type="submit"
            className={clsx(!ableToSubmit && "bg-[rgba(48,54,89,0.5)]", "rounded bg-deepKoamaru py-1.5 px-3 mt-4 text-white flex justify-center items-center gap-x-2 text-sm md:text-base")}>
            Lưu
        </button>
    </form>
};

const UserProfile = (): JSX.Element => {
    const {state, dispatch} = useAuthenticationContext();
    const [openDialog, setOpenDialog] = useState<boolean>(false);

    const baseUrl = useBaseUrl();

    const handleCloseDialog = () => {
        setOpenDialog(!openDialog);
    };

    return <div className="flex flex-col items-center justify-start mx-auto md:w-[400px] max-w-full">
        <span className="w-full flex flex-col justify-start mt-4 items-center">
            {state.user.avatar !== "" ?
                <Avatar imageUrl={baseUrl + state.user.avatar} className="w-28 h-28 sm:w-40 sm:h-40"/> :
                <Avatar className="w-28 h-28 sm:w-40 sm:h-40"/>}

            <span className="font-bold text-xl mt-2">{state.user.firstName} {state.user.lastName}</span>
            <span className="font-normal text-sm">@{state.user.userName}</span>
        </span>

        <button onClick={() => setOpenDialog(true)}
                className="rounded bg-deepKoamaru py-2.5 px-3 mt-4 text-white flex justify-center items-center gap-x-2 text-sm md:text-base">
            <BsPencil/> <span>Sửa thông tin</span>
        </button>

        <ul className="mt-12 self-start text-base sm:text-lg">
            <li className="flex flex-row gap-x-2 items-center">
                <span className="w-4 h-4 bg-oldRose block shrink-0 self-start mt-1"></span>
                <span className="font-semibold">
                    Email: <span className="text-oldRose">{state.user.email}</span>
                </span>
            </li>

            <li className="flex flex-row gap-x-2 items-center">
                <span className="w-4 h-4 bg-oldRose block shrink-0 self-start mt-1"></span>
                <span className="font-semibold">
                    Đã đọc: <span className="text-oldRose">0</span> truyện, <span
                    className="text-oldRose">0</span> chương
                        </span>
            </li>

            <li className="flex flex-row gap-x-2 items-center">
                <span className="w-4 h-4 bg-oldRose block shrink-0 self-start mt-1"></span>
                <span className="font-semibold">
                    Bình luận: <span className="text-oldRose">0</span>
                </span>
            </li>
        </ul>

        <Dialog
            width={"400px"}
            open={openDialog}
            handleClose={handleCloseDialog}>
            <InfoChangingForm/>
        </Dialog>
    </div>
}

export {UserProfile};