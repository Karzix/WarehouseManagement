using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace WarehouseManagement.DAL.Models.Entity
{
    public class Supplier
    {
        public Guid Id { get; set; }
        public string? SupplierName { get; set; }
        public string? SupplierAddress { get; set;}
        public string? SupplierPhoneNumber { get; set; }
        public string? SupplierEmail { get; set; }
        public string? ProductType { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime UpdatedDate { get; set;}
    }
}
