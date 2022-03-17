using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace AutenticacionService.Domain.Models
{
    public class UserTechnicianCreate : UserBaseCreate
    {
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
