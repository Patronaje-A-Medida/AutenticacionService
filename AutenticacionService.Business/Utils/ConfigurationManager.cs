using Microsoft.Extensions.Configuration;

namespace AutenticacionService.Business.Utils
{
    public class ConfigurationManager : IConfigurationManager
    {
        private readonly IConfiguration _configuration;

        public ConfigurationManager(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public string FirebaseApiKey
        {
            get
            {
                return _configuration["FirebaseSettings:ApiKey"];
            }
        }

        public string FirebaseBucket => _configuration["FirebaseSettings:Bucket"];

        public string FirebaseAuthEmail => _configuration["FirebaseSettings:AuthEmail"];

        public string FirebaseAuthPwd => _configuration["FirebaseSettings:AuthPwd"];
    }
}
