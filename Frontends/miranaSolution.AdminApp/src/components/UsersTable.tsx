import { Paper, Table, TableBody, TableCell, TableContainer, TableHead, TableRow } from "@mui/material";
import React from "react";
import { useQuery } from "react-query";
import { booksApiClient } from "../helpers/apis/BooksApiClient";

function createData(
  name: string,
  calories: number,
  fat: number,
  carbs: number,
  protein: number,
) {
  return {name, calories, fat, carbs, protein};
}

const rows = [
  createData('Frozen yoghurt', 159, 6.0, 24, 4.0),
  createData('Ice cream sandwich', 237, 9.0, 37, 4.3),
  createData('Eclair', 262, 16.0, 24, 6.0),
  createData('Cupcake', 305, 3.7, 67, 4.3),
  createData('Gingerbread', 356, 16.0, 49, 3.9),
];

const UsersTable = (): JSX.Element => {

  const {isLoading, isError, data, error} = useQuery({
    queryKey: [ 'books' ],
    queryFn: () => booksApiClient.getPaging(),
  });

  if (isLoading || isError || error || !data) {
    return <></>;
  }

  return <TableContainer component={Paper}>
    <Table sx={{minWidth: 650}} aria-label="simple table">
      <TableHead>
        <TableRow>
          <TableCell>Id</TableCell>
          <TableCell>Name</TableCell>
          <TableCell>Author</TableCell>
        </TableRow>
      </TableHead>
      <TableBody>
        {data!.items.map((row) => (
          <TableRow
            key={row.id}
            sx={{'&:last-child td, &:last-child th': {border: 0}}}
          >
            <TableCell component="th" scope="row">
              {row.name}
            </TableCell>
            <TableCell>{row.name}</TableCell>
            <TableCell>{row.authorName}</TableCell>
          </TableRow>
        ))}
      </TableBody>
    </Table>
  </TableContainer>;
};

export { UsersTable };