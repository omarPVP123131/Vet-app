using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System;

namespace VeterinaryClinicManagement.Models
{
    public class Patient
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Species { get; set; } = string.Empty;
        public DateTime DateOfBirth { get; set; }
        public string OwnerName { get; set; } = string.Empty;
        public string ContactNumber { get; set; } = string.Empty;
        public bool IsVaccinated { get; set; }
        public bool HasPendingPayments { get; set; }
    }
}

