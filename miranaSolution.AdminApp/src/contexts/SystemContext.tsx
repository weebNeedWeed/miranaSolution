import { createContext, useContext, useEffect, useReducer } from "react";

export enum SystemActionType {
  SET_PAGE_NAME = "SET_PAGE_NAME",
}

type SystemState = {
  pageName: string;
};
type Action = {
  type: SystemActionType.SET_PAGE_NAME,
  payload: string
};
type Dispatch = (action: Action) => void;

type SystemContextProps = { state: SystemState, dispatch: Dispatch };
const SystemContext = createContext<SystemContextProps>({} as SystemContextProps);

const systemReducer = (state: SystemState, action: Action) => {
  switch (action.type) {
    case SystemActionType.SET_PAGE_NAME:
      state.pageName = action.payload;
      return {...state};
    default:
      return state;
  }
};

export const useSystemContext = (): SystemContextProps => {
  const context = useContext<SystemContextProps>(SystemContext);
  if (!context) {
    throw new Error("useSystemContext must be used within SystemContextProvider");
  }

  return context;
};

const initialState: SystemState = {
  pageName: ""
};

export const SystemContextProvider = ({children}: { children: React.ReactNode }): JSX.Element => {
  const [ state, dispatch ] = useReducer(systemReducer, initialState);

  return <SystemContext.Provider value={{state, dispatch}}>
    {children}
  </SystemContext.Provider>;
};