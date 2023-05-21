import { MdOutlineClose } from "react-icons/md";
import {clsx} from "clsx";
import {AiOutlineCheckCircle} from "react-icons/ai";
import {BiErrorCircle} from "react-icons/bi";
import {AiOutlineWarning} from "react-icons/ai";
import {BsInfoCircle} from "react-icons/bs";

export type ToastMessage = {
  id?: string;
  title: string;
  variant?: ToastVariant;
};

export enum ToastVariant {
  Success,
  Error,
  Warning,
  Info
}

type ToastProps = {
  className?: string;
  toast: ToastMessage;
  removeToastById: () => void
}

function getIconWithColor(icon: React.ReactNode, color: string) {
  return <div className={"flex flex-row items-center text-lg font-bold"} style={{color}}>
    {icon}
  </div>
}

const matchingIconsForVariants = {
  [ToastVariant.Success]: getIconWithColor(<AiOutlineCheckCircle />,"#28d785"),
  [ToastVariant.Error]: getIconWithColor(<BiErrorCircle />, "#e05260"),
  [ToastVariant.Warning]:  getIconWithColor(<AiOutlineWarning />, "#ffcc33"),
  [ToastVariant.Info]: getIconWithColor(<BsInfoCircle />,"#3dd6f5"),
}

const Toast = (props: ToastProps) : JSX.Element => {
  const {className, toast, removeToastById} = props;
  const finalClassName = clsx("min-h-[3.5rem] w-full flex justify-center items-stretch bg-deepKoamaru p-2 text-darkVanilla shadow-md shadow-deepKoamaru rounded-sm", className);

  const toastVariant: ToastVariant = toast.variant ?? ToastVariant.Success;

  const handleRemoveToast = (event: React.MouseEvent<unknown> ) => {
    removeToastById();
  }

  return <div className={finalClassName}>
    <div className="flex flex-row items-center text-lg">
      {matchingIconsForVariants[toastVariant]}
    </div>
    <p className="mx-2 grow w-full break-all flex items-center text-base">
      {toast.title}
    </p>
    <div className="flex items-start">
      <button className="outline-none border-0 font-bold" onClick={handleRemoveToast}>
        <MdOutlineClose />
      </button>
    </div>
  </div>
}

export { Toast };