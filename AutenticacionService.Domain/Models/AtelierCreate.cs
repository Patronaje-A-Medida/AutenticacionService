using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace AutenticacionService.Domain.Models
{
    public class AtelierCreate
    {
        [Required(ErrorMessage = "El Nombre del Atelier no puede ser nulo")]
        [MaxLength(100, ErrorMessage = "El Nombre del Atelier no puede ser mayor de 100 caracteres")]
        public string NameAtelier { get; set; }

        [Required(ErrorMessage = "El RUC no puede ser nulo")]
        [MaxLength(11, ErrorMessage = "El RUC debe contener 11 caracteres")]
        [MinLength(11, ErrorMessage = "El RUC debe contener 11 caracteres")]
        public string RucAtelier { get; set; }

        [Required(ErrorMessage = "La Ciudad no puede ser nula")]
        [MaxLength(100, ErrorMessage = "La Ciudad no puede ser mayor de 100 caracteres")]
        public string City { get; set; }

        [Required(ErrorMessage = "El Distrito no puede ser nulo")]
        [MaxLength(100, ErrorMessage = "El Distrito no puede ser mayor de 100 caracteres")]
        public string District { get; set; }

        [Required(ErrorMessage = "La Dirección no puede ser nula")]
        [MaxLength(100, ErrorMessage = "La Dirección no puede ser mayor de 100 caracteres")]
        public string Address { get; set; }

        [Required(ErrorMessage = "La Descripción del Atelier no puede ser nula")]
        public string DescriptionAtelier { get; set; }
    }
}
