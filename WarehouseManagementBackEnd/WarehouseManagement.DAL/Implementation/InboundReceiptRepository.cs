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
    public class InboundReceiptRepository : GenericRepository<InboundReceipt, WarehouseManagementDbContext>, IInboundReceiptRepository
    {
        public InboundReceiptRepository(WarehouseManagementDbContext unitOfWork) : base(unitOfWork)
        {
            _context = unitOfWork;
        }
    }
}
