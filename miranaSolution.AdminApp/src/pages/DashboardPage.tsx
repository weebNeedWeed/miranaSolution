import { SystemActionType, useSystemContext } from "../contexts/SystemContext";
import { useEffect } from "react";

const DashboardPage = (): JSX.Element => {
  const {state, dispatch} = useSystemContext();
  useEffect(() => {
    dispatch({type: SystemActionType.SET_PAGE_NAME, payload: "Home"});
  }, []);

  return <>
    aa
  </>;
};

export { DashboardPage };