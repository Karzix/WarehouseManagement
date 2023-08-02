using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WarehouseManagement.DAL.Models.Entity
{
    public class Lot
    {
        public Guid Id { get; set; }
        public int Quantily { get; set; }
        public DateTime DateAdded { get; set; }
        public int PurchasePrice { get; set; }

        [ForeignKey("Supplier")]
        public Guid SupplierId { get; set; }
        [ForeignKey("SupplierId")]
        public virtual Supplier? Supplier { get; set; }

        [ForeignKey("Product")]
        public Guid ProductId { get; set; }
        [ForeignKey("ProductId")]
        public virtual Product? Product { get; set; }
    }
}
