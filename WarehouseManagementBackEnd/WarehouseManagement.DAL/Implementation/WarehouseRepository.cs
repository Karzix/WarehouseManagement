using Maynghien.Common.Repository;
using WarehouseManagement.DAL.Contract;
using WarehouseManagement.DAL.Models.Context;
using WarehouseManagement.DAL.Models.Entity;

namespace WarehouseManagement.DAL.Implementation
{
    public class WarehouseRepository : GenericRepository<Warehouse, WarehouseManagementDbContext>, IWarehouseRepository
    {
        public WarehouseRepository(WarehouseManagementDbContext unitOfWork) : base(unitOfWork)
        {
            _context = unitOfWork;
        }
    }
}
