using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MayNghien.Common.Models.Entity;

namespace WarehouseManagement.DAL.Models.Entity
{
    public class Warehouse:BaseEntity
    {
        public string? Name { get; set; }
        public string? Address { get; set; }
        public string? Managent { get; set; }
        public string? Email { get; set; }

    }
}
