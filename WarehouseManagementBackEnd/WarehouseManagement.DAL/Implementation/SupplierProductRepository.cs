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
    public class SupplierProductRepositor : GenericRepository<SupplierProduct, WarehouseManagementDbContext>, ISupplierProductRepository
    {
        public SupplierProductRepositor(WarehouseManagementDbContext unitOfWork) : base(unitOfWork)
        {
            _context = unitOfWork;
        }

        public SupplierProduct FindById(Guid Id)
        {
            var supplierProduct = _context.SupplierProduct.Where(x => x.Id == Id)
                .Include(x=>x.Supplier)
                .Include(x=>x.Product)
                .FirstOrDefault();
            return supplierProduct;
        }
    }
}
