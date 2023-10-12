using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Maynghien.Common.Repository;
using WarehouseManagement.DAL.Models.Context;
using WarehouseManagement.DAL.Models.Entity;

namespace WarehouseManagement.DAL.Contract
{
    public interface IProductRemainingRepository:IGenericRepository<ProductRemaining, WarehouseManagementDbContext>
    {
        ProductRemaining FindById(int id);
    }
}
