import { act } from "@testing-library/react";
import { UserState, UserActions, UserActionTypes } from "./types";

const initialState: UserState = {
  user: {},
  message: null,
  loading: false,
  error: null,
  isAuth: false,
  users: [],
  roles: []
};

const UserReducer = (state = initialState, action: UserActions): UserState => {
  switch (action.type) {
    case UserActionTypes.GETROLES_ACTION:
      return { ...state, loading: true };
    case UserActionTypes.GETROLES_SUCCESS:
      return { ...state, message: action.payload.message, roles: action.payload.payload, loading: false };
    case UserActionTypes.GETROLES_ERROR:
      return { ...state, message: action.payload.message };
    case UserActionTypes.LOGIN_USER:
      return { ...state, loading: true };
    case UserActionTypes.REGISTER_USER:
      return { ...state, loading: true };
    case UserActionTypes.REGISTER_SUCCESS:
      return { ...state, loading: false };
    case UserActionTypes.REGISTER_ERROR:
      return { ...state, loading: false, message: action.payload.message };
    case UserActionTypes.GETALL_USER:
      return { ...state, loading: true };
    case UserActionTypes.GETALL_SUCCESS:
      return { ...state, loading: false, roles: action.payload.roles };
    case UserActionTypes.GETALL_ERROR:
      return { ...state, loading: false, message: action.payload.message }
    case UserActionTypes.LOGIN_USER_SUCCESS:
      return {
        ...state,
        isAuth: true,
        loading: false,
        user: action.payload.decodedToken,
        message: action.payload.message,
        roles: action.payload.roles
      };
    case UserActionTypes.LOGIN_USER_ERROR:
      return { ...state, loading: false, message: action.payload.message }
    case UserActionTypes.SERVER_USER_ERROR:
      return { ...state, loading: false, message: action.payload.message };
    case UserActionTypes.LOGOUT_USER:
      return {
        ...state,
        isAuth: false,
        loading: false,
        user: null,
        message: null,
        error: null,
        users: [],
        roles: []
      };
    case UserActionTypes.FORGOT_USER_PASSWORD:
      return { ...state, loading: true };
    case UserActionTypes.FORGOT_USER_PASSWORD_SUCCESS:
      return { ...state, loading: false, message: action.payload.message };
    case UserActionTypes.FORGOT_USER_PASSWORD_ERROR:
      return { ...state, loading: false, message: action.payload.message };
    default:
      return state;
  }
};

export default UserReducer;
