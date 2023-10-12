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
        public int SupplierId { get; set; }
        public string? SupplierName { get; set; }
        public int WarehouseId { get; set; }
        public string? WarehousName { get; set; }

        public List<ImportProductDto> ListImportProductDto { get; set; }
    }
}
