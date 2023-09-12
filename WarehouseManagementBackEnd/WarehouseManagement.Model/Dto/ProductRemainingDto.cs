using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MayNghien.Common.Models;

namespace WarehouseManagement.Model.Dto
{
    public class ProductRemainingDto:BaseDto
    {
        public int Quantity { get; set; }

        public Guid ProductId { get; set; }
        public string? ProductName { get; set; }

        public Guid WarehouseId { get; set; }
        public string? WarehouseName { get; set; }
    }
}
