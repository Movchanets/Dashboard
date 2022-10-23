import React from "react";
// import React Router Dom
import { Routes, Route } from "react-router-dom";

import { useTypedSelector } from "./hooks/useTypedSelector";

// Import components
import Login from "./pages/auth/login";
import NotFound from "./pages/notFound";
import ForgotPassword from "./pages/auth/forgotPassword";
import DashboardLayout from "./containers/dashboardLayout";
import DefaultPage from "./pages/defaultPage";
import Users from "./pages/users";
import RegisterUser from './pages/auth/RegisterUser';

const App: React.FC = () => {
  // For protected routes
  const { isAuth, user } = useTypedSelector((store) => store.UserReducer);

  return (
    <Routes>
      {isAuth && (
        <>
          {user.role === "Administrators" && (
            <Route path="/dashboard" element={<DashboardLayout />}>
              <Route index element={<DefaultPage />} />
              <Route path="users" element={<Users />} />
              <Route path="register" element={<RegisterUser />} />
            </Route>
          )}
          {user.role === "Users" && (
            <Route path="/dashboard" element={<DashboardLayout />}>
              <Route index element={<DefaultPage />} />
            </Route>
          )}
        </>
      )}

      <Route path="/" element={<Login />} />
      <Route path="/forgotPassword" element={<ForgotPassword />} />

      <Route path="/dashboard" element={<Login />} />
      <Route path="*" element={<NotFound />} />
    </Routes>
  );
};

export default App;
