using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AutenticacionService.Domain.Base
{
    public class UserBase : IdentityUser
    {
        [Required]
        [Column(TypeName = "nvarchar(100)")]
        public string NameUser { get; set; }

        [Required]
        [Column(TypeName = "nvarchar(100)")]
        public string LastNameUser { get; set; }

        [Required]
        [Column(TypeName = "nvarchar(20)")]
        public string Role { get; set; }

        [Required]
        [Column(TypeName = "bit")]
        public bool EmailVerified { get; set; }

        [Required]
        [Column(TypeName = "nvarchar(20)")]
        public string Status { get; set; }
    }
}
