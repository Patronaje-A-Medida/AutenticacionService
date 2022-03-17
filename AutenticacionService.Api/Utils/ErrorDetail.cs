using Newtonsoft.Json;

namespace AutenticacionService.Api.Utils
{
    public class ErrorDetail
    {
        public int statusCode { get; set; }
        public int errorCode { get; set; }
        public string message { get; set; }

        public override string ToString()
        {
            return JsonConvert.SerializeObject(this);
        }
    }
}
