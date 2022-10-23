export interface UserState {
    user: any,
    message: null | string,
    loading: boolean,
    error: null | string
    isAuth: boolean
    users: any
    roles: any
}

export enum UserActionTypes {
    LOGIN_USER = "LOGIN_USER",
    GETALL_USER = "GETALL_USER",
    GETALL_SUCCESS = "GETALL_SUCCESS",
    GETALL_ERROR = "GETALL_ERROR",
    LOGIN_USER_ERROR = "LOGIN_USER_ERROR",
    LOGIN_USER_SUCCESS = "LOGIN_USER_SUCCESS",
    FORGOT_USER_PASSWORD = "FORGOT_USER_PASSWORD",
    FORGOT_USER_PASSWORD_SUCCESS = "FORGOT_USER_PASSWORD_SUCCESS",
    FORGOT_USER_PASSWORD_ERROR = "FORGOT_USER_PASSWORD_ERROR",
    SERVER_USER_ERROR = "SERVER_USER_ERROR",
    LOGOUT_USER = "LOGOUT_USER",
    REGISTER_USER = "REGISTER_USER",
    REGISTER_SUCCESS = "REGISTER_SUCCESS",
    REGISTER_ERROR = "REGISTER_ERROR",
    GETROLES_ACTION = "GETROLES_ACTION",
    GETROLES_SUCCESS = "GETROLES_SUCCESS",
    GETROLES_ERROR = "GETROLES_ERROR",
}
interface GetRolesAction {
    type: UserActionTypes.GETROLES_ACTION
}
interface GetRolesActionSuccess {
    type: UserActionTypes.GETROLES_SUCCESS,
    payload: any
}
interface GetRolesActionError {
    type: UserActionTypes.GETROLES_ERROR,
    payload: any
}
interface RegisterUserAction {
    type: UserActionTypes.REGISTER_USER
}
interface RegisterUserActionSuccess {
    type: UserActionTypes.REGISTER_SUCCESS,
    payload: any
}
interface RegisterUserActionError {
    type: UserActionTypes.REGISTER_ERROR,
    payload: any
}
interface LOGOUT_USER {
    type: UserActionTypes.LOGOUT_USER
}

interface ForgotUserPasswordAction {
    type: UserActionTypes.FORGOT_USER_PASSWORD
}

interface ForgotUserPasswordSuccessAction {
    type: UserActionTypes.FORGOT_USER_PASSWORD_SUCCESS,
    payload: any
}

interface ForgotUserPasswordErrorAction {
    type: UserActionTypes.FORGOT_USER_PASSWORD_ERROR,
    payload: any
}

interface LoginUserAction {
    type: UserActionTypes.LOGIN_USER,
}
interface GetAllUserAction {
    type: UserActionTypes.GETALL_USER,
}
interface GetAllUserSuccess {
    type: UserActionTypes.GETALL_SUCCESS, payload: any
}
interface GetAllUserError {
    type: UserActionTypes.GETALL_ERROR, payload: any
}
interface LoginUserSuccessAction {
    type: UserActionTypes.LOGIN_USER_SUCCESS
    payload: any
}

interface LoginUserErrorAction {
    type: UserActionTypes.LOGIN_USER_ERROR
    payload: any
}

interface ServerUserErrorAction {
    type: UserActionTypes.SERVER_USER_ERROR,
    payload: any
}

export type UserActions = LOGOUT_USER
    | ForgotUserPasswordErrorAction
    | LoginUserErrorAction
    | LoginUserAction
    | ServerUserErrorAction
    | LoginUserSuccessAction
    | ForgotUserPasswordAction
    | ForgotUserPasswordSuccessAction
    | GetAllUserSuccess
    | GetAllUserError
    | GetAllUserAction
    | RegisterUserAction
    | RegisterUserActionSuccess
    | RegisterUserActionError
    | GetRolesAction
    | GetRolesActionError
    | GetRolesActionSuccess