using System;
using System.Collections.Generic;
using System.Text;

namespace AutenticacionService.Domain.Models
{
    public class UserAtelierToken
    {
        public UserAtelierRead UserInfo { get; set; }
        public UserToken UserToken { get; set; }
    }
}
