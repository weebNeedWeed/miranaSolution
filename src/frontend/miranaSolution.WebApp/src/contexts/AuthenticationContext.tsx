import {createContext, useContext, useReducer} from "react";
import {User} from "../helpers/models/auth/User";

type State = {
    user: User
};
type Action = { type: "setUserData", payload: User };
type Dispatch = (action: Action) => void

type AuthenticationContextProps = {
    state: State,
    dispatch: Dispatch
}
const AuthenticationContext = createContext<AuthenticationContextProps>({} as AuthenticationContextProps);

const authenticationReducer = (state: State, action: Action): State => {
    switch (action.type) {
        case "setUserData":
            const cloned = {...state};
            cloned.user = action.payload;

            return cloned;
        default:
            return {...state};
    }
}

const initializeValues: State = {
    user: {} as User
};
export const AuthenticationContextProvider = (props: { children: React.ReactNode }): JSX.Element => {
    const {children} = props;
    const [state, dispatch] = useReducer(authenticationReducer, initializeValues);

    return <AuthenticationContext.Provider value={{state, dispatch}}>
        {children}
    </AuthenticationContext.Provider>;
}

export const useAuthenticationContext = (): AuthenticationContextProps => {
    const authenticationContext = useContext(AuthenticationContext);
    if (authenticationContext === undefined) {
        throw new Error("useAuthenticationContext must be used within AuthenticationContext");
    }

    return authenticationContext;
}