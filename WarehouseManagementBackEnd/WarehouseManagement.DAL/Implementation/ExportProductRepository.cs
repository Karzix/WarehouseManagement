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
    public class ExportProductRepository : GenericRepository<ExportProduct, WarehouseManagementDbContext>, IExportProductRepository
    {
        public ExportProductRepository(WarehouseManagementDbContext unitOfWork) : base(unitOfWork)
        {
            _context = unitOfWork;
        }

        public ExportProduct FindById(int Id)
        {
            var exportProduct = _context.ExportProduct.Where(x => x.Id == Id)
                .Include(x => x.Supplier)
                .Include(x => x.Product)
                .FirstOrDefault();
            return exportProduct;
        }
    }
}
