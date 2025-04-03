using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Models
{
    public class AuditTrail
    {
        public DateTime CreateDate { get; set; }
        public DateTime? UpdateDate { get; set; }

        [DisplayName("Is Active")]
        public bool IsActive { get; set; }
    }
}
