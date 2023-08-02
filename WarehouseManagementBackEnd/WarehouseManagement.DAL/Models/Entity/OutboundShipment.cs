using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WarehouseManagement.DAL.Models.Entity
{
    public class OutboundShipment
    {
        public Guid Id { get; set; }
        public int Quantity { get; set; }
        public DateTime DateShipped { get; set; }
        public string? OutboundShipmentClerk { get; set; }

        [ForeignKey("Product")]
        public Guid ProductId { get; set; }
        [ForeignKey("ProductId")]
        public virtual Product? Product { get; set; }

    }
}
