using System;
using System.Collections.Generic;
using System.Text;

namespace AutenticacionService.Domain.Models
{
    public class UserClientUpdate
    {
        public int Id { get; set; }
        public string UserId { get; set; }
        public string NameUser { get; set; }
        public string LastNameUser { get; set; }
        public decimal Height { get; set; }
        public string Phone { get; set; }
    }
}
