using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace AutenticacionService.Domain.Models
{
    public class UserTechnicianCreate
    {
        [Required(ErrorMessage = "El Correo no puede ser nulo")]
        [EmailAddress]
        [MaxLength(100, ErrorMessage = "La Contraseña no puede ser mayor de 100 caracteres")]
        public string Email { get; set; }

        [Required(ErrorMessage = "El Nombre del usuario no puede ser nulo")]
        [MaxLength(100, ErrorMessage = "El Nombre no puede ser mayor de 100 caracteres")]
        public string NameUser { get; set; }

        [Required(ErrorMessage = "Los Apellidos del usuario no pueden ser nulo")]
        [MaxLength(100, ErrorMessage = "Los Apellidos no peuden ser mayor de 100 caracteres")]
        public string LastNameUser { get; set; }

        [Required(ErrorMessage = "El DNI no puede ser nulo")]
        [MaxLength(8, ErrorMessage = "El DNI debe contener 8 caracteres")]
        [MinLength(8, ErrorMessage = "El DNI debe contener 8 caracteres")]
        public string Dni { get; set; }

        [Required(ErrorMessage = "El Id del Jefe no puede ser nulo")]
        public int BossId { get; set; }

        [Required(ErrorMessage = "El Id del Atelier no puede ser nulo")]
        public int AtelierId { get; set; }
    }
}
