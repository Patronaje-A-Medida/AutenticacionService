namespace AutenticacionService.Domain.Models
{
    public class UserBaseRead
    {
        public string UserId { get; set; }
        public string Email { get; set; }
        public string NameUser { get; set; }
        public string LastNameUser { get; set; }
        public string Role { get; set; }
    }
}
