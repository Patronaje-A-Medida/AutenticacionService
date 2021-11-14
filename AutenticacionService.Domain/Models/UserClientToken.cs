using System;
using System.Collections.Generic;
using System.Text;

namespace AutenticacionService.Domain.Models
{
    public class UserClientToken : UserToken
    {
        public int Id { get; set; }
        public string Email { get; set; }
        public string NameUser { get; set; }
        public string LastNameUser { get; set; }
        public decimal Height { get; set; }
        public string Phone { get; set; }
        public string Role { get; set; }
    }
}
