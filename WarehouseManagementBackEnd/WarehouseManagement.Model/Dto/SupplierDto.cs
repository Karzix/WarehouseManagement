using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MayNghien.Common.Models;

namespace WarehouseManagement.Model.Dto
{
    public class SupplierDto:BaseDto
    {
        public string Name { get; set; }
        public string? Email { get; set; }
    }
}
