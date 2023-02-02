import { useLayoutEffect } from "react";
import { Loading } from "../components";
import { useSystemContext } from "../contexts/SystemContext";
import { useLocation } from "react-router-dom";

const LoadingScreen = (): JSX.Element => {
	const { state, dispatch } = useSystemContext();
	const location = useLocation();

	useLayoutEffect(() => {
		if (state.showLoading === false) {
			dispatch({ type: "startLoading" });

			const temp = setTimeout(() => {
				dispatch({ type: "endLoading" });
			}, 3000);

			return () => {
				clearTimeout(temp);
			};
		}
	}, [location]);

	return <Loading show={state.showLoading} />;
};

export { LoadingScreen };
