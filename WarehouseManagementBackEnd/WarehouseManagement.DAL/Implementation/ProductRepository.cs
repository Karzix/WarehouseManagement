﻿using System;
using System.Collections.Generic;
using System.Linq;
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
    }
}