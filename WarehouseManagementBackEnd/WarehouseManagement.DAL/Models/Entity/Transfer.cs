using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WarehouseManagement.DAL.Models.Entity
{
    public class Transfer
    {
        public Guid Id { get; set; }
        public int Quantity { get; set; }
        public DateTime DateTransferred { get; set; }
        public string? TransferClerk { get; set; }


        [ForeignKey("Product")]
        public Guid ProductId { get; set; }
        [ForeignKey("ProductId")]
        public virtual Product? Product { get; set; }

    }
}
