using Newtonsoft.Json;

namespace AutenticacionService.Api.Utils
{
    public class ErrorDevDetail
    {
        public int statusCode { get; set; }
        public int errorCode { get; set; }
        public string message { get; set; }
        public string systemMessage { get; set; }

        public override string ToString()
        {
            return JsonConvert.SerializeObject(this);
        }
    }
}
