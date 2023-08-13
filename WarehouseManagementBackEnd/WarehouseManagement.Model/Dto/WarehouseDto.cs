using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MayNghien.Common.Models;

namespace WarehouseManagement.Model.Dto
{
    public class WarehouseDto: BaseDto
    {
        public string? Name { get; set; }
        public string? Address { get; set; }
        public string? Managent { get; set; }
        public string? Email { get; set; }
    }
}
