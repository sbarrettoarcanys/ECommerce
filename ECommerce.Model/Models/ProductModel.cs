using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

namespace ECommerce.Models.Models
{
    public class ProductModel : AuditTrail
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Code { get; set; }

        public double Price { get; set; }
        public double? DiscountedPrice { get; set; }

        [ValidateNever]
        public List<ProductImageModel> ProductImages { get; set; }

        [ValidateNever]
        public List<ProductCategoryModel> ProductCategories { get; set; } = [];

        [ValidateNever]
        public List<CategoryModel> Categories { get; set; } = [];
    }
}
