using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MayNghien.Common.Models.Entity;

namespace WarehouseManagement.DAL.Models.Entity
{
    public class ExportProduct: BaseEntity
    {
        public int Quantity { get; set; }

        [ForeignKey("Product")]
        public Guid ProductId { get; set; }
        [ForeignKey("ProductId")]
        public Product? Product { get; set; }

        [ForeignKey("Supplier")]
        public Guid SupplierId { get; set; }
        [ForeignKey("SupplierId")]
        public Supplier Supplier { get; set; }

        [ForeignKey("OutboundReceipt")]
        public Guid OutboundReceiptId { get; set; }
        [ForeignKey("OutboundReceiptId")]
        public OutboundReceipt OutboundReceipt { get; set; }
    }
}
