using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WarehouseManagement.DAL.Models.Entity
{
    public class InboundReceipt
    {
        public Guid Id { get; set; }
        public DateTime DateAdded { get; set; }
        public string? InboundReceiptClerk { get; set; }

        [ForeignKey("Lot")]
        public Guid LotId { get; set; }
        [ForeignKey("LotId")]
        public virtual Lot? Lot { get; set; }
    }
}
