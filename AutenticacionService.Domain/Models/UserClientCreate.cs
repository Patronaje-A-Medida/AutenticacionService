using System.ComponentModel.DataAnnotations;

namespace AutenticacionService.Domain.Models
{
    public class UserClientCreate : UserBaseCreate
    {
        [Required(ErrorMessage = "La Altura no puede ser nula")]
        public decimal Height { get; set; }

        [Required(ErrorMessage = "El Teléfono no puede ser nulo")]
        [MaxLength(13, ErrorMessage = "Número de Teléfono incorrecto")]
        public string Phone { get; set; }
    }
}
