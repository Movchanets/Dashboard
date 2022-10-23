import React, {useEffect} from "react";
import Table from "@mui/material/Table";
import TableBody from "@mui/material/TableBody";
import TableCell from "@mui/material/TableCell";
import TableContainer from "@mui/material/TableContainer";
import TableHead from "@mui/material/TableHead";
import TableRow from "@mui/material/TableRow";
import Paper from "@mui/material/Paper";
import { useActions } from "../../hooks/useActions";
import { Button } from "@mui/material";
import { useTypedSelector } from "../../hooks/useTypedSelector";







const Users: React.FC = () => {
  const { GetUsers } = useActions();
useEffect( () => {
  GetUsers();
},[]);
const { users } = useTypedSelector((store) => store.UserReducer);

  return(
    <>
    <TableContainer component={Paper}>
      <Table sx={{ minWidth: 650 }} aria-label="simple table">
        <TableHead>
          <TableRow>
            <TableCell align="center">UserName</TableCell>
            <TableCell align="center">Name</TableCell>
            <TableCell align="center">Surname</TableCell>
            <TableCell align="center">Email</TableCell>
            <TableCell align="center">Confirm Email</TableCell>
            <TableCell align="center">Role</TableCell>
          </TableRow>
        </TableHead>
        <TableBody>
        {users.map((user:any) => (
            <TableRow 
              key={user.email}
              sx={{ "&:last-child td, &:last-child th": { border: 0 } }}
            >
              <TableCell component="th" scope="row" align="center">
                {user.userName}
              </TableCell>
              <TableCell align="center">{user.name}</TableCell>
              <TableCell align="center">{user.surname}</TableCell>
              <TableCell align="center">{user.email}</TableCell>
              <TableCell align="center">{user.emailConfirmed?"True":"False"}</TableCell>
              <TableCell align="center">{user.role}</TableCell>
            </TableRow>
          ))}
        </TableBody>
      </Table>
    </TableContainer></>
  );
};

export default Users;
