using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore.Query.Internal;

namespace WarehouseManagement.DAL.Models.Entity
{
    public class InventoryReport
    {
        public Guid Id { get; set; }

        [ForeignKey("Product")]
        public Guid ProductId { get; set; }
        [ForeignKey("ProductId")]
        public virtual Product? Product { get; set; }

        [ForeignKey("Supplier")]
        public Guid SupplierId { get; set; }
        [ForeignKey("SupplierId")]
        public virtual Supplier? Supplier { get; set; }
    }
}
