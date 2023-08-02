using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WarehouseManagement.DAL.Models.Entity
{
    public class Warehouse
    {
        public Guid Id { get; set; }
        public string? WarehouseName { get; set; }
        public string? WarehouseAddress { get; set; }
        public int PhoneNumber { get; set; }
        public string? WarehouseManager { get; set; }
    }
}
