import { UserState, UserActions, UserActionTypes } from "./types";

const initialState: UserState = {
  user: {},
  message: null,
  loading: false,
  error: null,
  isAuth: false,
};

const UserReducer = (state = initialState, action: UserActions): UserState => {
  switch (action.type) {
    case UserActionTypes.LOGIN_USER:
      return { ...state, loading: true };
    case UserActionTypes.LOGIN_USER_SUCCESS:
      return {
        ...state,
        isAuth: true,
        loading: false,
        user: action.payload.decodedToken,
        message: action.payload.message,
      };
    case UserActionTypes.LOGIN_USER_ERROR:
        return {...state, loading: false, message: action.payload.message}
    case UserActionTypes.SERVER_USER_ERROR:
      return { ...state, loading: false, message: action.payload.message };
    case UserActionTypes.LOGOUT_USER:
      return {
        isAuth: false,
        loading: false,
        user: null,
        message: null,
        error: null
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
