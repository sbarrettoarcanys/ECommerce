using ECommerce.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Models.ViewModels
{
    public class ProductCategoryViewModel : AuditTrail
    {
        public int Id { get; set; }

        public int ProductId { get; set; }
        public ProductViewModel ProductViewmodel { get; set; }

        public int CategoryId { get; set; }
        public CategoryViewModel CategoryViewModel { get; set; }
    }
}
