import { SystemActionType, useSystemContext } from "../contexts/SystemContext";
import React, { useEffect } from "react";
import { Box, Button, Table, TableContainer, TableRow, TableHead, TableCell, TableBody } from "@mui/material";

const UsersPage = (): JSX.Element => {
  const {state, dispatch} = useSystemContext();
  useEffect(() => {
    dispatch({type: SystemActionType.SET_PAGE_NAME, payload: "Users"});
  }, []);
  return <>
    <Button variant="contained" color="success">
      + Create new
    </Button>
  </>;
};

export { UsersPage };