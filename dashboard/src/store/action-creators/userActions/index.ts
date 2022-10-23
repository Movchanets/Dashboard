import { UserActionTypes, UserActions } from "../../reducers/userReducer/types";
import { Dispatch } from "redux";
import { toast } from "react-toastify";
import { login, forgotPassword, removeTokens, GetAllUsers, RegisterUser, GetAllRoles } from "../../../services/api-user-service";
import jwtDecode from "jwt-decode";
import {
  setAccessToken,
  setRefreshToken,
} from "../../../services/api-user-service";
import { Toast } from 'react-toastify/dist/components';
export const GetUsers = () => {
  return async (dispatch: Dispatch<UserActions>) => {
    try {
      dispatch({ type: UserActionTypes.GETALL_USER });
      const data = await GetAllUsers();
      const { response } = data;
      if (!response.isSuccess) {
        dispatch({
          type: UserActionTypes.GETALL_ERROR,
          payload: data.response.message,
        });
        toast.error(response.message);
      } else {
        console.log(response);

        dispatch({
          type: UserActionTypes.GETALL_SUCCESS,
          payload: response.payload,
        });


      }
    } catch (e) {
      dispatch({
        type: UserActionTypes.SERVER_USER_ERROR,
        payload: "Unknown error",
      });
    }
  };
};
export const GetRoles = () => {
  return async (dispatch: Dispatch<UserActions>) => {
    try {
      dispatch({ type: UserActionTypes.GETROLES_ACTION });
      const data = await GetAllRoles();
      const { response } = data;
      if (!response.isSuccess) {
        dispatch({
          type: UserActionTypes.GETROLES_ERROR,
          payload: data.response.message,
        });
        toast.error(response.message);
      } else {
        console.log(response);

        dispatch({
          type: UserActionTypes.GETROLES_SUCCESS,
          payload: response,
        });


      }
    } catch (e) {
      dispatch({
        type: UserActionTypes.SERVER_USER_ERROR,
        payload: "Unknown error",
      });
    }
  };
};
export const LoginUser = (user: any) => {
  return async (dispatch: Dispatch<UserActions>) => {
    try {
      dispatch({ type: UserActionTypes.LOGIN_USER });
      const data = await login(user);
      const { response } = data;
      if (!response.isSuccess) {
        dispatch({
          type: UserActionTypes.LOGIN_USER_ERROR,
          payload: data.response.message,
        });
        toast.error(response.message);
      } else {
        const { accessToken, refreshToken, message, payload } = data.response;
        console.log(payload);

        setAccessToken(accessToken);
        setRefreshToken(refreshToken);
        AuthUser(accessToken, message, dispatch);
      }
    } catch (e) {
      dispatch({
        type: UserActionTypes.SERVER_USER_ERROR,
        payload: "Unknown error",
      });
    }
  };
};
export const registerUser = (user: any) => {
  return async (dispatch: Dispatch<UserActions>) => {
    try {
      dispatch({ type: UserActionTypes.REGISTER_USER });
      console.log(user);
      const data = await RegisterUser(user);
      const { response } = data;
      if (!response.isSuccess) {
        dispatch({
          type: UserActionTypes.REGISTER_ERROR,
          payload: data.response.message,
        });
        toast.error(response.message);
      } else {
        dispatch({
          type: UserActionTypes.REGISTER_SUCCESS,
          payload: data.response.message,
        });
        toast(data.response.message);
      }
    } catch (e) {
      dispatch({
        type: UserActionTypes.SERVER_USER_ERROR,
        payload: "Unknown error",
      });
    }
  };
};
export const ForgotPassword = (email: string) => {
  return async (dispatch: Dispatch<UserActions>) => {
    try {
      dispatch({ type: UserActionTypes.FORGOT_USER_PASSWORD });
      const data = await forgotPassword(email);
      console.log("ForgotPassword ", data);
      const { response } = data;
      console.log("ForgotPassword ", response);
      if (!response.isSuccess) {
        dispatch({
          type: UserActionTypes.FORGOT_USER_PASSWORD_ERROR,
          payload: data.response,
        });
        toast.error(response.message);
      } else {
        dispatch({
          type: UserActionTypes.FORGOT_USER_PASSWORD_SUCCESS,
          payload: data.response,
        });
      }
    } catch (e) {
      dispatch({
        type: UserActionTypes.SERVER_USER_ERROR,
        payload: "Unknown error",
      });
    }
  };
};

export const LogOut = () => {
  return async (dispatch: Dispatch<UserActions>) => {
    removeTokens();
    dispatch({
      type: UserActionTypes.LOGOUT_USER
    });
  }
}

export const AuthUser = (token: string, message: string, dispatch: Dispatch<UserActions>) => {
  const decodedToken = jwtDecode(token) as any;
  dispatch({
    type: UserActionTypes.LOGIN_USER_SUCCESS,
    payload: {
      message,

      decodedToken
    },
  });
};
