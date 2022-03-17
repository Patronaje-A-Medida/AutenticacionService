namespace AutenticacionService.Domain.Models
{
    public class UserClientToken
    {
        public UserClientRead UserInfo { get; set; }
        public UserToken UserToken { get; set; }
    }
}
