using AutenticacionService.Domain.Base;

namespace AutenticacionService.Domain.Entities
{
    public class UserAtelier : Auditable
    {
        public int Id { get; set; }
        public string Role { get; set; }
        public string Dni { get; set; }
        public int? BossId { get; set; }

        // relations
        public string UserId { get; set; }
        public UserBase User { get; set; }
        public int AtelierId { get; set; }
        public Atelier Atelier { get; set; }
    }
}
