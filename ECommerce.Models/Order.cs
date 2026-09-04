using System;
using System.Collections.Generic;
using System.Text;

namespace ECommerce.Models
{
    public class Order
    {
        public int OrderId { get; set; }

        public int UserId { get; set; }

        public DateTime OrderDate { get; set; }

        public decimal TotalAmount { get; set; }

        public string Status { get; set; }= string.Empty;
    }

}
