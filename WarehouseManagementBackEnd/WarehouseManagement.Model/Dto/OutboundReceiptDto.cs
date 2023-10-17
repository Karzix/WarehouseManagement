using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MayNghien.Common.Models;

namespace WarehouseManagement.Model.Dto
{
    public class OutboundReceiptDto:BaseDto
    {
        public string? To { get; set; }

        public int WarehouseId { get; set; }
        public string? WarehouseName { get; set; }
		public DateTime? CreatedOn { get; set; }

		public List<ExportProductDto> ListExportProductDtos { get; set; }
    }
}
