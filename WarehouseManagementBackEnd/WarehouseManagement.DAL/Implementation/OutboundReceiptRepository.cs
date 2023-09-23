using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Maynghien.Common.Repository;
using Microsoft.EntityFrameworkCore;
using WarehouseManagement.DAL.Contract;
using WarehouseManagement.DAL.Models.Context;
using WarehouseManagement.DAL.Models.Entity;

namespace WarehouseManagement.DAL.Implementation
{
    public class OutboundReceiptRepository : GenericRepository<OutboundReceipt, WarehouseManagementDbContext>, IOutboundReceiptRepository
    {
        public OutboundReceiptRepository(WarehouseManagementDbContext unitOfWork) : base(unitOfWork)
        {
            _context = unitOfWork;
        }

        public OutboundReceipt FindById(Guid id)
        {
            var outboundReceipt = _context.OutboundReceipt.Where(x=>x.Id == id)
                .Include(x=>x.Warehouse)
                .First();
            return outboundReceipt;
        }
    }
}
