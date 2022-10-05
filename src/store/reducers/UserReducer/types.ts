export interface UserState {
  user: any;
  loading: boolean;
  error: null | string;
  accessToken: null | string;
  refreshToken: null | string;
  isAuth: null | boolean;
  message: null | string;
}

export enum UserActionTypes {
  LOGIN_USER = "LOGIN_USER",
  LOGIN_USER_SUCCESS = "LOGIN_USER_SUCCESS ",
  LOGIN_USER_ERROR = "LOGIN_USER_ERROR",
  LOGOUT_USER = "LOGOUT_USER",
  REQUEST_ACTION = "REQUEST_ACTION_SUCCESS",
  REQUEST_ACTION_ERROR = "REQUEST_ACTION_ERROR",
  FORGOT_PASSWORD_SUCCESS = "FORGOT_PASSWORD_SUCCESS",
}

interface ForgotPasswordAction {
  type: UserActionTypes.FORGOT_PASSWORD_SUCCESS;
  payload: any;
}

interface UserRequestActionSuccess {
  type: UserActionTypes.REQUEST_ACTION;
}

interface UserRequestActionError {
  type: UserActionTypes.REQUEST_ACTION_ERROR;
  payload: any;
}

interface LogoutUserAction {
  type: UserActionTypes.LOGOUT_USER;
}

interface LoginUserAction {
  type: UserActionTypes.LOGIN_USER;
}

interface LoginUserSuccessAction {
  type: UserActionTypes.LOGIN_USER_SUCCESS;
  payload: any;
}

interface LoginUserErrorAction {
  type: UserActionTypes.LOGIN_USER_ERROR;
  payload: any;
}

export type UserActions =
  | LoginUserAction
  | LoginUserSuccessAction
  | LoginUserErrorAction
  | UserRequestActionSuccess
  | UserRequestActionError
  | LogoutUserAction
  | ForgotPasswordAction;
