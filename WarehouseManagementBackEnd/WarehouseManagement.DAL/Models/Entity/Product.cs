using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WarehouseManagement.DAL.Models.Entity
{
    public class Product
    {
        public Guid Id { get; set; }
        public string? ProductName { get; set; }
        public string? ProductDescription { get; set; }
        public int PurchasePrice { get; set; }
        public int SellingPrice { get; set; }
        public int QuantityOnHand { get; set;}
        public DateTime DateAdded { get; set; }
        public DateTime DateUpdated { get; set; }

        [ForeignKey("Supplier")]
        public Guid SupplierId { get; set; }
        [ForeignKey("SupplierId")]
        public virtual Supplier? Supplier { get; set; }

    }
}
