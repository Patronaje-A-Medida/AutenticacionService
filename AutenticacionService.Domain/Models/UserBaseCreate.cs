using System.ComponentModel.DataAnnotations;

namespace AutenticacionService.Domain.Models
{
    public class UserBaseCreate
    {
        [Required(ErrorMessage = "El Correo no puede ser nulo")]
        [EmailAddress]
        [MaxLength(100, ErrorMessage = "La Contraseña no puede ser mayor de 100 caracteres")]
        public string Email { get; set; }

        [Required(ErrorMessage = "La Contraseña no puede ser nula")]
        [MaxLength(100, ErrorMessage = "La Contraseña no puede ser mayor de 100 caracteres")]
        public string Password { get; set; }

        [Required(ErrorMessage = "El Nombre del usuario no puede ser nulo")]
        [MaxLength(100, ErrorMessage = "El Nombre no puede ser mayor de 100 caracteres")]
        public string NameUser { get; set; }

        [Required(ErrorMessage = "Los Apellidos del usuario no pueden ser nulo")]
        [MaxLength(100, ErrorMessage = "Los Apellidos no peuden ser mayor de 100 caracteres")]
        public string LastNameUser { get; set; }
    }
}
