using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MayNghien.Common.Models;

namespace WarehouseManagement.Model.Dto
{
    public class ImportProductDto :BaseDto
    {
        public int Quantity { get; set; }

        public int SupplierId { get; set; }
        public string? SupplierName { get; set; }
        public int ProductId { get; set; }
        public string? ProductName { get; set; }
		//public DateTime? CreatedOn { get; set; }

		public int? InboundReceiptId { get; set; }
    }
}
