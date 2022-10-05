import { UserActions, UserActionTypes, UserState } from "./types";

const initialState: UserState = {
  user: {},
  loading: false,
  error: null,
  accessToken: null,
  refreshToken: null,
  isAuth: false,
  message: null,
};

const UserReducer = (state = initialState, action: UserActions): UserState => {
  switch (action.type) {
    case UserActionTypes.REQUEST_ACTION:
      return { ...state, loading: true };
    case UserActionTypes.REQUEST_ACTION_ERROR:
      return { ...state, loading: false, error: action.payload.message };
    case UserActionTypes.LOGIN_USER_SUCCESS:
      return {
        isAuth: true,
        loading: false,
        error: null,
        user: action.payload.message,
        accessToken: action.payload.accessToken,
        refreshToken: action.payload.refreshToken,
        message: action.payload.message,
      };
    case UserActionTypes.LOGIN_USER_ERROR:
      return { ...state, loading: false,message: action.payload.message, error: action.payload.error };
    case UserActionTypes.LOGOUT_USER:
      return {
        error: null,
        message: null,
        user: null,
        loading: false,
        isAuth: false,
        accessToken: null,
        refreshToken: null,
      };
    case UserActionTypes.FORGOT_PASSWORD_SUCCESS:
      return { ...state, loading: false, message: action.payload.message };
    default:
      return state;
  }
};

export default UserReducer;
