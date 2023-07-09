import {BsPencil} from "react-icons/bs";
import React, {useState} from "react";
import {useAuthenticationContext} from "../../contexts/AuthenticationContext";
import {Dialog, TextInput} from "../../components";
import {FaLock} from "react-icons/fa";
import clsx from "clsx";
import {userApiHelper} from "../../helpers/apis/UserApiHelper";
import {ToastVariant} from "../../components/Toast";
import {useAccessToken} from "../../helpers/hooks/useAccessToken";
import {useSystemContext} from "../../contexts/SystemContext";
import {useNavigate} from "react-router-dom";

const PasswordChangingForm = (): JSX.Element => {
    const [ableToSubmit, setAbleToSubmit] = useState<boolean>(false)
    const [oldPassword, setOldPassword] = useState<string>("");
    const [newPassword, setNewPassword] = useState<string>("");
    const [newPasswordConfirmation, setNewPasswordConfirmation] = useState<string>("");
    const [accessToken, setAccessToken] = useAccessToken();
    const {state: systemState, dispatch: systemDispatch} = useSystemContext();
    const navigate = useNavigate();

    const validateForm = (oldPassword: string, newPassword: string, newPasswordConfirmation: string) => {
        if (oldPassword.trim().length === 0) {
            setAbleToSubmit(false);
            return;
        }

        if (newPassword.trim().length === 0) {
            setAbleToSubmit(false);
            return;
        }

        if (newPasswordConfirmation.trim().length === 0) {
            setAbleToSubmit(false);
            return;
        }

        setAbleToSubmit(true);
    };

    const handleOldPasswordChange = (event: React.ChangeEvent<HTMLInputElement>) => {
        const _oldPassword = event.target.value;
        validateForm(_oldPassword, newPassword, newPasswordConfirmation);
        setOldPassword(_oldPassword);
    }

    const handleNewPasswordChange = (event: React.ChangeEvent<HTMLInputElement>) => {
        const _newPassword = event.target.value;
        validateForm(oldPassword, _newPassword, newPasswordConfirmation);
        setNewPassword(_newPassword);
    }

    const handleNewPasswordConfirmationChange = (event: React.ChangeEvent<HTMLInputElement>) => {
        const _newPasswordConfirmation = event.target.value;
        validateForm(oldPassword, newPassword, _newPasswordConfirmation);
        setNewPasswordConfirmation(_newPasswordConfirmation);
    }

    const handleSubmit = async (event: React.FormEvent<HTMLFormElement>) => {
        event.preventDefault();
        if (!ableToSubmit) {
            return;
        }

        const result = await userApiHelper.updatePassword(accessToken, {
            oldPassword,
            newPassword,
            newPasswordConfirmation
        });

        if (result === null) {
            systemDispatch({
                type: "addToast", payload: {
                    title: "Có lỗi xảy ra! Vui lòng thử lại.",
                    variant: ToastVariant.Error
                }
            });
        } else {
            systemDispatch({
                type: "addToast", payload: {
                    title: "Cập nhật mật khẩu thành công.",
                    variant: ToastVariant.Success
                }
            })

            // Reload after 2 secs
            setTimeout(() => {
                navigate(0);
            }, 2000);
        }
    }

    return <form onSubmit={handleSubmit} className="w-full">
        <div className="w-full">
            <TextInput
                label={"Mật khẩu cũ"}
                icon={<FaLock/>}
                placeholder={"Nhập mật khẩu cũ"}
                type="password"
                value={oldPassword}
                onChange={handleOldPasswordChange}
            />
        </div>

        <div className="w-full mt-4">
            <TextInput
                label={"Mật khẩu mới"}
                icon={<FaLock/>}
                placeholder={"Nhập mật khẩu mới"}
                type="password"
                value={newPassword}
                onChange={handleNewPasswordChange}
            />
        </div>

        <div className="w-full mt-4">
            <TextInput
                label={"Nhập lại mật khẩu mới"}
                icon={<FaLock/>}
                placeholder={"Nhập lại mật khẩu mới"}
                type="password"
                value={newPasswordConfirmation}
                onChange={handleNewPasswordConfirmationChange}
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

const UserPassword = (): JSX.Element => {
    const [openDialog, setOpenDialog] = useState<boolean>(false);
    const {state, dispatch} = useAuthenticationContext();

    const handleCloseDialog = () => {
        setOpenDialog(!openDialog);
    };

    return <div className="flex flex-col items-start justify-start mx-auto md:w-[400px] max-w-full">
        <ul className="self-start text-base sm:text-lg">
            <li className="flex flex-row gap-x-2 items-center">
                <span className="w-4 h-4 bg-oldRose block shrink-0 self-start mt-1"></span>
                <span className="font-semibold">
                    Mật khẩu: <span className="text-oldRose">**********</span>
                </span>
            </li>
        </ul>

        <button onClick={() => setOpenDialog(true)}
                className="item rounded bg-deepKoamaru py-2.5 px-3 mt-4 text-white flex justify-center items-center gap-x-2 text-sm md:text-base">
            <BsPencil/> <span>Đổi mật khẩu</span>
        </button>

        <Dialog
            width={"400px"}
            open={openDialog}
            handleClose={handleCloseDialog}>
            <PasswordChangingForm/>
        </Dialog>
    </div>
}

export {UserPassword};