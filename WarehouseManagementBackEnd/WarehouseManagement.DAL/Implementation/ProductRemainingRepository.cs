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
    public class ProductRemainingRepository : GenericRepository<ProductRemaining, WarehouseManagementDbContext>, IProductRemainingRepository
    {
        public ProductRemainingRepository(WarehouseManagementDbContext unitOfWork) : base(unitOfWork)
        {
            _context = unitOfWork;
        }

        public ProductRemaining FindById(int id)
        {
            var productRemaining = _context.ProductRemaining.Where(x => x.Id == id)
                .Include(x => x.Product)
                .Include(x=>x.Warehouse)
                .FirstOrDefault();
            return productRemaining;
        }
    }
}
