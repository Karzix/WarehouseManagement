using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Maynghien.Common.Repository;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using WarehouseManagement.DAL.Contract;
using WarehouseManagement.DAL.Models.Context;
using WarehouseManagement.DAL.Models.Entity;
using WarehouseManagement.Model.Dto;

namespace WarehouseManagement.DAL.Implementation
{
    public class SupplierProductRepositor : GenericRepository<SupplierProduct, WarehouseManagementDbContext>, ISupplierProductRepository
    {
        public SupplierProductRepositor(WarehouseManagementDbContext unitOfWork) : base(unitOfWork)
        {
            _context = unitOfWork;
        }

        public SupplierProduct FindById(int Id)
        {
            var supplierProduct = _context.SupplierProduct.Where(x => x.Id == Id)
                .Include(x=>x.Supplier)
                .Include(x=>x.Product)
                .FirstOrDefault();
            return supplierProduct;
        }

        public List<ProductDto> FindProductBySupplier(int SupplierId)
        {
            var listSupplierProduct = _context.SupplierProduct.Where(x => x.SupplierId == SupplierId)
                .Include(x=>x.Product);
            var listProduct = listSupplierProduct.Select(x=> new ProductDto
            {
                Quantity = x.Product.Quantity,
                Description = x.Product.Description,
                Id = x.ProductId,
                Name = x.Product.Name,
            }).ToList();
            return listProduct;
        }
        public IQueryable<SupplierProduct> FindByPredicate(Expression<Func<SupplierProduct, bool>> predicate)
        {
            return _context.SupplierProduct.Where(predicate).AsQueryable();
        }
        public int CountRecordsByPredicate(Expression<Func<SupplierProduct, bool>> predicate)
        {
            return _context.SupplierProduct.Where(predicate).Count();
        }
    }
}
