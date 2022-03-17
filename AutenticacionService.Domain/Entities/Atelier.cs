using AutenticacionService.Domain.Base;
using System.Collections.Generic;

namespace AutenticacionService.Domain.Entities
{
    public class Atelier : Auditable
    {
        public int Id { get; set; }
        public string NameAtelier { get; set; }
        public string RucAtelier { get; set; }
        public string City { get; set; }
        public string District { get; set; }
        public string Address { get; set; }
        public string DescriptionAtelier { get; set; }

        // relations
        public IEnumerable<UserAtelier> Employees { get; set; }
    }
}
