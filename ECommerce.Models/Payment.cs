using System;
using System.Collections.Generic;
using System.Text;

namespace ECommerce.Models
{
    public class Payment
    {
        public int PaymentId { get; set; }

        public int OrderId { get; set; }

        public decimal Amount { get; set; }

        public string PaymentStatus { get; set; }= string.Empty;

        public DateTime PaymentDate { get; set; }
    }
}
