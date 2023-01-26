import { useContext, useReducer, useState, createContext } from "react";

type Action = { type: "startLoading" } | { type: "endLoading" };
type State = { showLoading: boolean };
type Dispatch = (action: Action) => void;

type SystemContextType = { state: State; dispatch: Dispatch };

const SystemContext = createContext<SystemContextType>({} as SystemContextType);

const SystemReducer = (state: State, action: Action): State => {
	switch (action.type) {
		case "startLoading": {
			return { ...state, showLoading: true };
		}

		case "endLoading": {
			return { ...state, showLoading: false };
		}

		default: {
			return state;
		}
	}
};

const initialState: State = {
	showLoading: false,
};

export const SystemContextProvider = ({
	children,
}: {
	children: React.ReactNode;
}): JSX.Element => {
	const [state, dispatch] = useReducer(SystemReducer, initialState);

	return (
		<SystemContext.Provider value={{ state, dispatch }}>
			{children}
		</SystemContext.Provider>
	);
};

export const useSystemContext = (): SystemContextType => {
	const systemContext = useContext(SystemContext);

	if (SystemContext === undefined) {
		throw new Error(
			"useSystemContext must be use within SystemContextProvider",
		);
	}

	return systemContext;
};
