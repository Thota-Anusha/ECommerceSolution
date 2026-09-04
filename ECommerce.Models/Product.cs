using System;
using System.Collections.Generic;
using System.Text;

namespace ECommerce.Models
{
    public class Product
    {
        public int ProductId { get; set; } 

        public string Name { get; set; }= string.Empty;

        public string Description { get; set; }

        public decimal Price { get; set; } 

        public int CategoryId { get; set; } 
    }
}
