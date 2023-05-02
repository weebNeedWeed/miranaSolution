import { SystemActionType, useSystemContext } from "../contexts/SystemContext";
import { useEffect } from "react";

const UsersPage = (): JSX.Element => {
  const {state, dispatch} = useSystemContext();
  useEffect(() => {
    dispatch({type: SystemActionType.SET_PAGE_NAME, payload: "Users"});
  }, []);
  return <div>bb</div>;
};

export { UsersPage };