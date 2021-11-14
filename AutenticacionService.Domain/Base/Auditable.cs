using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AutenticacionService.Domain.Base
{
    public abstract class Auditable
    {
        [Required]
        [Column(TypeName = "datetimeoffset(7)")]
        public DateTimeOffset CreatedDate { get; set; }

        [Column(TypeName = "nvarchar(100)")]
        public string CreatedBy { get; set; }

        [Column(TypeName = "datetimeoffset(7)")]
        public DateTimeOffset? ModifiedDate { get; set; }
        
        [Column(TypeName = "nvarchar(100)")]
        public string ModifiedBy { get; set; }
    }
}
