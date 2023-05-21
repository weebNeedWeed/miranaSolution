import { SystemActionType, useSystemContext } from "../../contexts/SystemContext";
import React, { useEffect } from "react";
import { Box, Button, Paper, Table, TableBody, TableCell, TableContainer, TableHead, TableRow } from "@mui/material";
import { UsersTable } from "../../components/UsersTable";
import { Link } from "react-router-dom";

const BooksIndexPage = (): JSX.Element => {
  const {state, dispatch} = useSystemContext();
  useEffect(() => {
    dispatch({type: SystemActionType.SET_PAGE_NAME, payload: "Books"});
  }, []);
  return <>
    <Link to="/dashboard/books/create">
      <Button variant="contained" color="success">
        + Create new
      </Button>
    </Link>

    <Box sx={{width: "100%", marginTop: "1rem"}}>
      <UsersTable/>
    </Box>
  </>;
};

export { BooksIndexPage };