using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WarehouseManager.Models
{
    class Product
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string ProductType { get; set; }
        public string Supplier { get; set; }
        public int Quantity { get; set; }
        public decimal Price { get; set; }
        public DateTime DeliveryDate { get; set; }
    }
}
