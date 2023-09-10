using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MayNghien.Common.Models;

namespace WarehouseManagement.Model.Dto
{
    public class ExportProductDto : BaseDto
    {
        public int Quantity { get; set; }

        public Guid SupplierProductId { get; set; }
        public string? SupplierProductName { get; set; }

        public Guid OutboundReceiptId { get; set; }
    }
}
