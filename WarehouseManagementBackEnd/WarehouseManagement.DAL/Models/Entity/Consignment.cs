using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MayNghien.Common.Models.Entity;

namespace WarehouseManagement.DAL.Models.Entity
{
    public class Consignment:BaseEntity
    {
        public string? From { get; set; }
        public string? To { get; set; }
        public bool ExportProduct { get; set; }

        [ForeignKey("Warehouse")]  
        public Guid WarehouseId { get; set; }
        [ForeignKey("WarehouseId")]
        public virtual Warehouse Warehouse { get; set; }
    }
}
