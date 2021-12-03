using System;
using System.Collections.Generic;
using System.Text;

namespace AutenticacionService.Domain.Models
{
    public class UserAtelierRead : UserBaseRead
    {
        public int Id { get; set; }
        public string Dni { get; set; }
        public int? BossId { get; set; }
        public int AtelierId { get; set; }
    }
}
