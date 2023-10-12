using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MayNghien.Common.Models.Entity;

namespace WarehouseManagement.DAL.Models.Entity
{
    public class SupplierProduct:BaseEntity
    {
        [ForeignKey("Product")]
        public int ProductId { get; set; }
        [ForeignKey("ProductId")]
        public Product? Product { get; set; }

        [ForeignKey("Supplier")]
        public int SupplierId { get; set;}
        [ForeignKey("SupplierId")]
        public Supplier Supplier { get; set; }
    }
}
