import { UserActions, UserActionTypes } from "../../reducers/UserReducer/types";
import { Dispatch } from "redux";
import { toast } from "react-toastify";
import { login, forgotPassword } from "../../../services/api-user-service";

export const LoginUser = (user: any) => {
  return async (dispatch: Dispatch<UserActions>) => {
    try {
      dispatch({ type: UserActionTypes.REQUEST_ACTION });
      const data = await login(user);
      const { responce } = data;
      console.log(responce);
      if (!responce.isSuccess) {
        dispatch({
          type: UserActionTypes.LOGIN_USER_ERROR,
          payload: responce.message,
        });
        toast.error(responce.message);
      } else {
        dispatch({
          type: UserActionTypes.LOGIN_USER_SUCCESS,
          payload: responce,
        });
     
      }
    } catch (error) {
      dispatch({
        type: UserActionTypes.REQUEST_ACTION_ERROR,
        payload: "Unknown error",
      });
    }
  };
};

export const ForgotPassword = (email: string) => {
  return async (dispatch: Dispatch<UserActions>) => {
    try {
      dispatch({ type: UserActionTypes.REQUEST_ACTION });
      const data = await forgotPassword(email);
      const { responce } = data;

      if (!responce.isSuccess) {
        dispatch({
          type: UserActionTypes.REQUEST_ACTION_ERROR,
          payload: responce.message,
        });
        toast.error(responce.message);
      } else {
        console.log("inside else");
        dispatch({
          type: UserActionTypes.FORGOT_PASSWORD_SUCCESS,
          payload: responce.message,
        });
        toast.success(responce.message);
      }
    } catch (error) {
      dispatch({
        type: UserActionTypes.REQUEST_ACTION_ERROR,
        payload: "Unknown error",
      });
    }
  };
};
