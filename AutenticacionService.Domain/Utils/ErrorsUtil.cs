﻿using System;
using System.Collections.Generic;
using System.Text;

namespace AutenticacionService.Domain.Utils
{
    public static class ErrorsUtil
    {
        public static class ErrorsCode
        {
            public static readonly int GENERIC_ERROR = 10000;
            public static readonly int ADD_CONTEXT_ERROR = 10001;
            public static readonly int INVALID_MODEL_ERROR = 10002;

            public static readonly int USER_REGISTER_FAILED = 10010;
            public static readonly int GET_USER_ERROR = 10011;
            public static readonly int LOGIN_USER_ERROR = 10012;
            public static readonly int LOGIN_USER_INVALID = 10013;
            public static readonly int SIGN_UP_ERROR = 10014;
            public static readonly int SIGN_UP_INVALID = 10015;
        }

        public static class ErrorMessages
        {
            public static readonly string GENERIC_ERROR_400 = "No se pudo obtener la información requerida";
            public static readonly string GENERIC_ERROR_500 = "Error Interno del Sistema";
            public static readonly string ADD_CONTEXT_ERROR = "No se pudo guardar el registro";

            public static readonly string USER_REGISTER_FAILED = "No se pudo registrar el usuario";
            public static readonly string GET_USER_ERROR = "No se pudo obtener la información del usuario requerido";
            public static readonly string LOGIN_USER_ERROR = "Error en el inicio de sesión del usuario";
            public static readonly string LOGIN_USER_INVALID = "Credenciales del usuario inválidas";
            public static readonly string SIGN_UP_ERROR = "Error en el registro de credenciales del usuario";
            public static readonly string SIGN_UP_INVALID = "No se pudo registrar las credenciales del usuario";
        }
    }
}

