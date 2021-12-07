using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AutenticacionService.Api.Utils
{
    public class ErrorDetail
    {
        public int StatusCode;
        public int ErrorCode;
        public string Message;

        public override string ToString()
        {
            return JsonConvert.SerializeObject(this);
        }
    }
}
