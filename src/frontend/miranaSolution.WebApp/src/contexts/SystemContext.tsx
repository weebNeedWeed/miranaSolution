import {createContext, useContext, useReducer} from "react";
import {ToastMessage} from "../components/Toast";
import {v4 as uuidv4} from "uuid";

type Action = { type: "startLoading" }
    | { type: "endLoading" }
    | { type: "addToast", payload: ToastMessage }
    | { type: "removeToast", payload: string };
type State = { showLoading: boolean, toast: Array<ToastMessage> };
type Dispatch = (action: Action) => void;

type SystemContextType = { state: State; dispatch: Dispatch };

const SystemContext = createContext<SystemContextType>({} as SystemContextType);

const SystemReducer = (state: State, action: Action): State => {
    switch (action.type) {
        case "startLoading": {
            return {...state, showLoading: true};
        }

        case "endLoading": {
            return {...state, showLoading: false};
        }

        case "addToast": {
            action.payload.id = uuidv4();
            const cloned = [...state.toast];
            cloned.push(action.payload);

            return {...state, toast: cloned};
        }

        case "removeToast": {
            const id = action.payload;
            let cloned = [...state.toast];
            cloned = cloned.filter(elm => elm.id !== id);

            return {...state, toast: cloned};
        }

        default: {
            return state;
        }
    }
};

const initialState: State = {
    showLoading: false,
    toast: []
};

export const SystemContextProvider = ({
                                          children,
                                      }: {
    children: React.ReactNode;
}): JSX.Element => {
    const [state, dispatch] = useReducer(SystemReducer, initialState);

    return (
        <SystemContext.Provider value={{state, dispatch}}>
            {children}
        </SystemContext.Provider>
    );
};

export const useSystemContext = (): SystemContextType => {
    const systemContext = useContext(SystemContext);

    if (SystemContext === undefined) {
        throw new Error(
            "useSystemContext must be used within SystemContextProvider"
        );
    }

    return systemContext;
};
