import * as Yup from "yup";

const passwordRegExp = /^(?=.*?[A-Z])(?=.*?[a-z])(?=.*?[0-9])(?=.*?[#?!@$%^&*-_]).{6,}$/;

export const LoginSchema = Yup.object().shape({
    email: Yup.string().email("Invalid email address").required("Required").label('Email address'),
    password: Yup.string()
      .min(6, "Password must be at least 6 characters")
      .required("Required").matches(passwordRegExp, "Password is not vaild").label("Password"),
  });

  export const ResetPasswordSchema = Yup.object().shape({
    email: Yup.string().email("Invalid email address").required("Required").label('Email address'),
  });
