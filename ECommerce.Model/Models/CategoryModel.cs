using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Models.Models
{
    public class CategoryModel : AuditTrail
    {
        [Key]
        public int Id { get; set; }

        [MaxLength(30)]
        public string Name { get; set; }
        public int DisplayOrder { get; set; }

        [ValidateNever]
        public List<ProductCategoryModel> ProductCategories { get; set; }

        [ValidateNever]
        public List<ProductModel> Products { get; set; }

    }
}
