import React from "react";
import ListItemButton from "@mui/material/ListItemButton";
import ListItemIcon from "@mui/material/ListItemIcon";
import ListItemText from "@mui/material/ListItemText";
import ListSubheader from "@mui/material/ListSubheader";
import PeopleIcon from "@mui/icons-material/People";
import BarChartIcon from "@mui/icons-material/BarChart";
import AssignmentIcon from "@mui/icons-material/Assignment";
import { Link } from "react-router-dom";
import { StepIcon } from '@mui/material';

export const adminMenu = (
  <React.Fragment>

    <ListItemButton>
      <ListItemIcon>
        <PeopleIcon />
      </ListItemIcon>
      <Link to="users">
        <ListItemText primary="Users" />
      </Link>
    </ListItemButton>
    <ListItemButton>
      <ListItemIcon>
        <AssignmentIcon />
      </ListItemIcon>
      <Link to="register">
        <ListItemText primary="RegisterUser" />
      </Link>
    </ListItemButton>
  </React.Fragment>
);

export const userMenu = (
  <React.Fragment>
    <ListItemButton>
      <ListItemIcon>
        <BarChartIcon />
      </ListItemIcon>
      <Link to="">
        <ListItemText primary="Dashboard" />
      </Link>
    </ListItemButton>


  </React.Fragment>
);
