import { Toast } from "../components";
import { ToastVariant } from "../components/Toast";
import {useSystemContext} from "../contexts/SystemContext";

const ToastContainer = () : JSX.Element => {
  const {state, dispatch} = useSystemContext();
  const removeToastById = (id: string) => () => {
    dispatch({type: "removeToast", payload: id});
  }

  return <div className="fixed top-0 right-0 w-[min(350px,100%)] p-3 z-50 flex flex-col gap-y-4">
    {state.toast.map((t) => <Toast key={t.id} toast={t} removeToastById={removeToastById(t.id!)}/>)}
  </div>
}

export {ToastContainer};