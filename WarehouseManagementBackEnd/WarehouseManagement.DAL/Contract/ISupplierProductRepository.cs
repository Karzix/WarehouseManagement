using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Maynghien.Common.Repository;
using Microsoft.AspNetCore.Identity;
using WarehouseManagement.DAL.Models.Context;
using WarehouseManagement.DAL.Models.Entity;
using WarehouseManagement.Model.Dto;

namespace WarehouseManagement.DAL.Contract
{
    public interface ISupplierProductRepository:IGenericRepository<SupplierProduct, WarehouseManagementDbContext>
    {
        SupplierProduct FindById(Guid Id);
        List<ProductDto> FindProductBySupplier(Guid SupplierId);
        IQueryable<SupplierProduct> FindByPredicate(Expression<Func<SupplierProduct, bool>> predicate);
        int CountRecordsByPredicate(Expression<Func<SupplierProduct, bool>> predicate);
    }
}
