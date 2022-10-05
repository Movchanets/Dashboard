// import React Router Dom
import { Routes, Route } from "react-router-dom";

// Import components
import Login from "./pages/auth/login";
import NotFound from "./pages/notFound";
import ForgotPassword from "./pages/auth/forgotPassword";
import Dashboard from "./pages/dashboard";
//get prop user reducer
import { useTypedSelector } from "./hooks/useTypedSelector";


const  App = ()=> {
  const {isAuth} = useTypedSelector((store) => store.UserReducer);
  return (
    <Routes>
      <Route path="/" element={<Login />} />
    {isAuth?<Route path="/dashboard" element={<Dashboard />} />:""}  
      
      <Route path="/forgotPassword" element={<ForgotPassword />} />
      <Route path="*" element={<NotFound />} />
      {/* <Route index element={<Home />} /> */}
    </Routes>
  );
}

export default App;
