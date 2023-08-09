using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MayNghien.Common.Models.Entity;

namespace WarehouseManagement.DAL.Models.Entity
{
    public class ImportProduct:BaseEntity
    {
        public int Quantity { get; set; }

        [ForeignKey("SupplierProduct")]
        public Guid SupplierProductId { get; set; }
        [ForeignKey("SupplierProductId")]
        public SupplierProduct SupplierProduct { get; set; }

        [ForeignKey("InboundReceipt")]
        public Guid InboundReceiptId { get; set; }
        [ForeignKey("InboundReceiptId")]
        public InboundReceipt InboundReceipt { get; set; }
    }
}
