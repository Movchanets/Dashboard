import { Email } from "@mui/icons-material";
import axios from "axios";

axios.defaults.baseURL = "https://localhost:5001/api/User";
axios.defaults.withCredentials = true;

const responseBody: any = (response: any) => response.data;

const requests = {
  get: (url: string) => axios.get(url).then().then(responseBody),
  post: (url: string, body?: any) =>
    axios.post(url, body).then().then(responseBody),
  put: (url: string, body?: string) =>
    axios.put(url, body).then().then(responseBody),
  patch: (url: string, body: string) =>
    axios.patch(url, body).then().then(responseBody),
  del: (url: string) => axios.delete(url).then().then(responseBody),
};

const User = {
  login: (user: any) => requests.post(`/login`, user),
  forgotPassword: (email: string) =>
    requests.get("/ForgotPassword/?email=" + email),
};

export async function login(user: any) {
  const data = await User.login(user)
    .then((responce) => {
      return {
        responce,
      };
    })
    .catch((error) => {
      return error.response;
    });
  return data;
}

export async function forgotPassword(email: string) {
  const data = await User.forgotPassword(email)
    .then((responce) => {
      return {
        responce,
      };
    })
    .catch((error) => {
      return error.response;
    });
  return data;
}
