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

     
    {isAuth?<Route path="/dashboard" element={<Dashboard />} />: 
    
     ""}  
      <Route path="/" element={<Login />} /> + ""+ 
      <Route path="/dashboard" element={<Login />} />
      <Route path="/forgotPassword" element={<ForgotPassword />} />
      
      <Route path="*" element={<NotFound />} />
     
    </Routes>
  );
}

export default App;
