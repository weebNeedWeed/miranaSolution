import {createContext, useContext, useReducer} from "react";
import {User} from "../helpers/models/catalog/user/User";

type State = {
    isLoggedIn: boolean,
    user: User
};
type Action = {
    type: "setUserData",
    payload: {
        user: User,
    }
} | {
    type: "setLoginStatus",
    payload: {
        status: boolean
    }
} | {
    type: "resetState"
}
type Dispatch = (action: Action) => void

type AuthenticationContextProps = {
    state: State,
    dispatch: Dispatch
}
const AuthenticationContext = createContext<AuthenticationContextProps>({} as AuthenticationContextProps);

const initializeValues: State = {
    isLoggedIn: false,
    user: {} as User
};

const authenticationReducer = (state: State, action: Action): State => {
    let cloned: State;
    switch (action.type) {
        case "setUserData":
            cloned = {...state};
            cloned.user = action.payload.user;

            return cloned;

        case "setLoginStatus":
            cloned = {...state};
            cloned.isLoggedIn = action.payload.status;

            return cloned;
        case "resetState":
            return {...initializeValues};
        default:
            return {...state};
    }
}

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