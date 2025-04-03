using ECommerce.Models.Models;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace ECommerce.Models.ViewModels
{
    public class ProductViewModel : AuditTrail
    {
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        public string Description { get; set; }
        [Required]
        public string Code { get; set; }

        [Required]
        [Display(Name = "Price")]
        [Range(1, 1000)]
        public double Price { get; set; }

        [Display(Name = "Discounted Price")]
        [Range(1, 1000)]
        public double? DiscountedPrice { get; set; }

        [ValidateNever]
        public List<ProductCategoryViewModel> ProductCategories { get; set; }
        [ValidateNever]
        public List<ProductImageViewModel> ProductImages { get; set; }

        [ValidateNever]
        public IEnumerable<SelectListItem> Categories { get; set; }

        [Display(Name = "Categories")]
        public IEnumerable<int> CategoryIds { get; set; }
    }

    public class IndexViewModel
    {
        [Display(Name = "Search")]
        public string SearchText { get; set; }

        public List<ProductViewModel> ProductViewModels { get; set; }
    }
}
