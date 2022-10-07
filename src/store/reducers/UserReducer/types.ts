export interface UserState{
  user: any,
  message: null | string,
  loading: boolean,
  error: null | string
  isAuth: boolean
}

export enum UserActionTypes{
  LOGIN_USER = "LOGIN_USER",
  LOGIN_USER_ERROR = "LOGIN_USER_ERROR",
  LOGIN_USER_SUCCESS = "LOGIN_USER_SUCCESS",
  FORGOT_USER_PASSWORD = "FORGOT_USER_PASSWORD",
  FORGOT_USER_PASSWORD_SUCCESS = "FORGOT_USER_PASSWORD_SUCCESS",
  FORGOT_USER_PASSWORD_ERROR = "FORGOT_USER_PASSWORD_ERROR",
  SERVER_USER_ERROR = "SERVER_USER_ERROR",
  LOGOUT_USER = "LOGOUT_USER"
}

interface LOGOUT_USER{
  type: UserActionTypes.LOGOUT_USER
}

interface ForgotUserPasswordAction{
  type: UserActionTypes.FORGOT_USER_PASSWORD
}

interface ForgotUserPasswordSuccessAction{
  type: UserActionTypes.FORGOT_USER_PASSWORD_SUCCESS,
  payload: any
}

interface ForgotUserPasswordErrorAction{
  type: UserActionTypes.FORGOT_USER_PASSWORD_ERROR,
  payload: any
}

interface LoginUserAction{
  type:UserActionTypes.LOGIN_USER,
}

interface LoginUserSuccessAction{
  type:UserActionTypes.LOGIN_USER_SUCCESS
  payload: any
}

interface LoginUserErrorAction{
  type:UserActionTypes.LOGIN_USER_ERROR
  payload: any
}

interface ServerUserErrorAction{
  type: UserActionTypes.SERVER_USER_ERROR,
  payload: any
}

export type UserActions = LOGOUT_USER | ForgotUserPasswordErrorAction | LoginUserErrorAction|LoginUserAction | ServerUserErrorAction | LoginUserSuccessAction | ForgotUserPasswordAction | ForgotUserPasswordSuccessAction