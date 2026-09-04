using System;
using System.Collections.Generic;
using System.Text;

namespace ECommerce.Models
{
    public class Inventory
    {
        public int InventoryId { get; set; }

        public int ProductId { get; set; }

        public int Quantity { get; set; }
    }
}
