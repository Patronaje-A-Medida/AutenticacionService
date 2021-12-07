using System;
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
            public static readonly int USER_REGISTER_FAILED = 10010;
        }

        public static class ErrorMessages
        {
            public static readonly string GENERIC_ERROR = "Error Interno del Sistema";
            public static readonly string ADD_CONTEXT_ERROR = "No se pudo guardar el registro en la base de datos";
            public static readonly string USER_REGISTER_FAILED = "No se pudo registrar el usuario";
        }
    }
}
