using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MayNghien.Common.Models;

namespace WarehouseManagement.Model.Dto
{
    public class InboundReceiptDto:BaseDto
    {
        public Guid SupplierId { get; set; }
        public string? SupplierName { get; set; }
        public Guid WarehouseId { get; set; }
        public string? WarehousName { get; set; }
    }
}
