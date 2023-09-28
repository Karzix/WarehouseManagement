using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Maynghien.Common.Repository;
using WarehouseManagement.DAL.Contract;
using WarehouseManagement.DAL.Models.Context;
using WarehouseManagement.DAL.Models.Entity;

namespace WarehouseManagement.DAL.Implementation
{
    public class ProductRepository : GenericRepository<Product, WarehouseManagementDbContext>, IProductRepository
    {
        public ProductRepository(WarehouseManagementDbContext unitOfWork) : base(unitOfWork)
        {
            _context = unitOfWork;
        }
        public IQueryable<Product> FindByPredicate(Expression<Func<Product, bool>> predicate)
        {
            return _context.Product.Where(predicate).AsQueryable();
        }
        public int CountRecordsByPredicate(Expression<Func<Product, bool>> predicate)
        {
            return _context.Product.Where(predicate).Count();
        }
    }
}
