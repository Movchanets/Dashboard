export interface UserState {
    user: any,
    message: null | string,
    loading: boolean,
    error: null | string
    isAuth: boolean,
    allUsers: [],
    roles: []
}

export enum UserActionTypes {
    START_REQUEST = "START_REQUEST",
    LOGIN_USER_ERROR = "LOGIN_USER_ERROR",
    LOGIN_USER_SUCCESS = "LOGIN_USER_SUCCESS",
    FORGOT_USER_PASSWORD_SUCCESS = "FORGOT_USER_PASSWORD_SUCCESS",
    FORGOT_USER_PASSWORD_ERROR = "FORGOT_USER_PASSWORD_ERROR",
    SERVER_USER_ERROR = "SERVER_USER_ERROR",
    LOGOUT_USER = "LOGOUT_USER",
    ALL_USERS_LOADED = "ALL_USERS_LOADED",
    GETROLES_SUCCESS = "GETROLES_SUCCESS",
    GETROLES_ERROR = "GETROLES_ERROR",
    REGISTER_SUCCESS = "REGISTER_SUCCESS",
    REGISTER_ERROR = "REGISTER_ERROR",
    PASSWORD_CHANGE_SUCCESS = "PASSWORD_CHANGE_SUCCESS",
    CHANGE_SUCCESS = "CHANGE_SUCCESS",
    CHANGE_ERROR = "CHANGE_ERROR",
    PASSWORD_CHANGE_ERROR = "PASSWORD_CHANGE_ERROR",
    UPDATE_USER = "UPDATE_USER",
    BLOCK_USER_SUCCESS = "BLOCK_USER_SUCCESS",
    BLOCK_USER_ERROR = "BLOCK_USER_ERROR",
    UNBLOCK_USER_SUCCESS = "UNBLOCK_USER_SUCCESS",
    UNBLOCK_USER_ERROR = "UNBLOCK_USER_ERROR",
    LOG_OUT_SUCCESS = "LOG_OUT_SUCCESS",
    LOG_OUT_ERROR = "LOG_OUT_ERROR",
    DELETE_USER_SUCCESS = "DELETE_USER_SUCCESS",
    DELETE_USER_ERROR = "DELETE_USER_ERROR",
    TOKEN_UPDATE_SUCCESS = "DELETE_USER_ERROR",
    TOKEN_UPDATE_ERROR = "DELETE_USER_ERROR",
}
interface UpdateTokenSuccess {
    type: UserActionTypes.TOKEN_UPDATE_SUCCESS,
    payload: any
}interface UpdateTokenError {
    type: UserActionTypes.TOKEN_UPDATE_ERROR,
    payload: any
}
interface DeleteUserSuccess {
    type: UserActionTypes.DELETE_USER_SUCCESS,
    payload: any
}interface DeleteUserError {
    type: UserActionTypes.DELETE_USER_ERROR,
    payload: any
}
interface LogOutUserSuccess {
    type: UserActionTypes.LOG_OUT_SUCCESS,
    payload: any
}
interface LogOutUserError {
    type: UserActionTypes.LOG_OUT_ERROR,
    payload: any
}
interface UnblockUserSuccess {
    type: UserActionTypes.UNBLOCK_USER_SUCCESS,
    payload: any
}
interface UnblockUserError {
    type: UserActionTypes.UNBLOCK_USER_ERROR,
    payload: any
}
interface BlockUserSuccess {
    type: UserActionTypes.BLOCK_USER_SUCCESS,
    payload: any
}
interface BlockUserError {
    type: UserActionTypes.BLOCK_USER_ERROR,
    payload: any
}
interface UpdateUser {
    type: UserActionTypes.UPDATE_USER,
    payload: any
}
interface ChangeUserPasswordActionSuccess {
    type: UserActionTypes.PASSWORD_CHANGE_SUCCESS,
    payload: any

}
interface ChangeUserPasswordActionError {
    type: UserActionTypes.PASSWORD_CHANGE_ERROR,
    payload: any

}
interface ChangeUserInfoActionSuccess {
    type: UserActionTypes.CHANGE_SUCCESS,
    payload: any

}
interface ChangeUserInfoActionError {
    type: UserActionTypes.CHANGE_ERROR,
    payload: any
}
interface RegisterUserActionSuccess {
    type: UserActionTypes.REGISTER_SUCCESS,
    payload: any
}
interface RegisterUserActionError {
    type: UserActionTypes.REGISTER_ERROR,
    payload: any
}
interface GetRolesActionSuccess {
    type: UserActionTypes.GETROLES_SUCCESS,
    payload: any
}
interface GetRolesActionError {
    type: UserActionTypes.GETROLES_ERROR,
    payload: any
}
interface LOGOUT_USER {
    type: UserActionTypes.LOGOUT_USER
}

interface ForgotUserPasswordSuccessAction {
    type: UserActionTypes.FORGOT_USER_PASSWORD_SUCCESS,
    payload: any
}

interface ForgotUserPasswordErrorAction {
    type: UserActionTypes.FORGOT_USER_PASSWORD_ERROR,
    payload: any
}

interface StartRequestAction {
    type: UserActionTypes.START_REQUEST,
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

interface AllUsersLoadedAction {
    type: UserActionTypes.ALL_USERS_LOADED,
    payload: any
}

export type UserActions = LOGOUT_USER
    | UpdateUser
    | RegisterUserActionSuccess
    | RegisterUserActionError
    | GetRolesActionError
    | GetRolesActionSuccess
    | ForgotUserPasswordErrorAction
    | LoginUserErrorAction
    | StartRequestAction
    | ServerUserErrorAction
    | LoginUserSuccessAction
    | ForgotUserPasswordSuccessAction
    | AllUsersLoadedAction
    | ChangeUserInfoActionSuccess
    | ChangeUserInfoActionError
    | ChangeUserPasswordActionError
    | ChangeUserPasswordActionSuccess
    | LogOutUserSuccess
    | LogOutUserError
    | UnblockUserSuccess
    | UnblockUserError
    | DeleteUserSuccess
    | DeleteUserError
    | UpdateTokenSuccess
    | UpdateTokenError
    | BlockUserError
    | BlockUserSuccess;