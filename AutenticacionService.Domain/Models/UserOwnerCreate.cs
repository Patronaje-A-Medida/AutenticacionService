using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace AutenticacionService.Domain.Models
{
    public class UserOwnerCreate : UserBaseCreate
    {
        [Required(ErrorMessage = "El DNI no puede ser nulo")]
        [MaxLength(8, ErrorMessage = "El DNI debe contener 8 caracteres")]
        [MinLength(8, ErrorMessage = "El DNI debe contener 8 caracteres")]
        public string Dni { get; set; }

        public AtelierCreate Atelier { get; set; }
    }
}
