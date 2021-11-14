using System.ComponentModel.DataAnnotations;

namespace AutenticacionService.Domain.Models
{
    public class UserLogin
    {
        [Required(ErrorMessage = "El Correo no puede ser nulo")]
        [EmailAddress]
        public string Email { get; set; }

        [Required(ErrorMessage = "La Contraseña no puede ser nula")]
        public string Password { get; set; }
    }
}
