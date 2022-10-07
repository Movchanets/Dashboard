import { useTypedSelector } from "../../../hooks/useTypedSelector";
import Table from '@mui/material/Table';
import TableBody from '@mui/material/TableBody';
import TableCell from '@mui/material/TableCell';
import TableContainer from '@mui/material/TableContainer';
import TableHead from '@mui/material/TableHead';
import TableRow from '@mui/material/TableRow';
import Paper from '@mui/material/Paper';
import React, {useState} from "react";
import Menu from "@mui/material/Menu";
import MenuItem from "@mui/material/MenuItem";
import { Button } from "@mui/material";
import ListItemText from '@mui/material/ListItemText';
export const UserInformation = ()=>
{
    const {user} = useTypedSelector((store) => store.UserReducer);
 
    return (
        <TableContainer component={Paper}>
        <Table sx={{ minWidth: 650 }} aria-label="simple table">
          <TableHead>
            <TableRow>
              <TableCell align="center">Email</TableCell>
              <TableCell align="center">Name</TableCell>
              <TableCell align="center">Surname</TableCell>
              <TableCell align="center">Role</TableCell>
              <TableCell align="center">EmailConfirmed</TableCell>
         
            </TableRow>
          </TableHead>
          <TableBody>
            
              <TableRow
                key={user.id}
                sx={{ '&:last-child td, &:last-child th': { border: 0 } }}
              >
                <TableCell component="th" align="center" scope="row">
                  {user.Email}
                </TableCell>
                <TableCell align="center">{user.Name}</TableCell>
                <TableCell align="center">{user.Surname}</TableCell>
                <TableCell align="center">{user.role}</TableCell>
                <TableCell align="center">{user.EmailConfirmed}</TableCell>
                
              </TableRow>
            
          </TableBody>
        </Table>
      </TableContainer>
    );
}
export const UserShowActions : React.FC = ()=>
{ const [anchorEl, setAnchorEl]: any = useState(null);
    const openProfileMenu = Boolean(anchorEl);
    const handleClick = (event: React.MouseEvent<HTMLButtonElement>) => {
      setAnchorEl(event.currentTarget);
    };
    const handleClose = () => {
      setAnchorEl(null);
      console.log("test")
    };
  
    const [open, setOpen] = useState(true);
    const toggleDrawer = () => {
      setOpen(!open);
    };

    return(         <div>
        {" "}
        <Button
          id="basic-button"
          aria-controls={open ? "basic-menu" : undefined}
          aria-haspopup="true"
          aria-expanded={open ? "true" : undefined}
          onClick={handleClick}
        >
         Users
        </Button>
        <Menu
          id="basic-menu"
          anchorEl={anchorEl}
          open={openProfileMenu}
          onClose={handleClose}
          MenuListProps={{
            "aria-labelledby": "basic-button",
          }}
        >
          <MenuItem>Profile</MenuItem>
          <MenuItem >Logout</MenuItem>
        </Menu>
      </div>);
}