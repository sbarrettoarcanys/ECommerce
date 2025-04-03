using ECommerce.Models.Models;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;

namespace ECommerce.Models.ViewModels
{
    public class ShoppingCartViewModel : AuditTrail
    {
        public int Id { get; set; }

        public int ProductId { get; set; }

        [ValidateNever]
        public ProductViewModel Product { get; set; }
        [Range(1, 1000, ErrorMessage = "Please enter a value between 1 and 1000")]
        public int Count { get; set; }

        public string ApplicationUserId { get; set; }

        [ValidateNever]
        public ApplicationUser ApplicationUser { get; set; }

        private double price;
        [NotMapped]
        public double Price 
        {
            get { return price; }
            set
            {
                price = value;
                PriceString = value.ToString("c");
            }
        }

        public string PriceString { get; set; }
    }

    public class ShoppingCartDetailsViewModel
    {
        public List<ShoppingCartViewModel> ShoppingCartViewModels { get; set; }

        private double orderTotal;
        public double OrderTotal
        {
            get { return orderTotal; }
            set
            {
                orderTotal = value;
                OrderTotalString = value.ToString("c");
            }
        }

        public string OrderTotalString { get; set; }

    }
}
