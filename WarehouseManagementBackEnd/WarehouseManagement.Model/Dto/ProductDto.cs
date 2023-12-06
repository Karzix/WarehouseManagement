using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MayNghien.Common.Models;

namespace WarehouseManagement.Model.Dto
{
    public class ProductDto:BaseDto
    {
        public string Name { get; set; }
        public string? Description { get; set; }
        public int? Quantity { get; set; }
    }
}
