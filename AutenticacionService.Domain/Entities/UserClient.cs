using AutenticacionService.Domain.Base;

namespace AutenticacionService.Domain.Entities
{
    public class UserClient : Auditable
    {
        public int Id { get; set; }
        public decimal Height { get; set; }
        public string Phone { get; set; }

        // relations
        public string UserId { get; set; }
        public UserBase User { get; set; }
    }
}
