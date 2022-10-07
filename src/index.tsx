import ReactDOM from "react-dom/client";
import { BrowserRouter } from "react-router-dom";
import { store } from "./store";
import { Provider } from "react-redux";
import App from "./App";
import { GetAccessToken } from "./services/api-user-service";
import { AuthUser  } from "./store/action-creators/UserActions";
const root = ReactDOM.createRoot(
  document.getElementById("root") as HTMLElement
);
const token = GetAccessToken();
if( token!=null)
{
  AuthUser(token, "Loaded from LC",store.dispatch);
}
root.render(
  <Provider store={store}>
    <BrowserRouter>
      <App />
    </BrowserRouter>
  </Provider>
);
