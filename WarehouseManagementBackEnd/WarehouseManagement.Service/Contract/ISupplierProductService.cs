﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MayNghien.Models.Response.Base;
using WarehouseManagement.Model.Dto;

namespace WarehouseManagement.Service.Contract
{
    public interface ISupplierProductService
    {
        AppResponse<List<SupplierProductDto>> GetAllSupplierProduct();
        AppResponse<SupplierProductDto> GetSupplierProduct(Guid Id);
        AppResponse<SupplierProductDto> CreateSupplierProduct(SupplierProductDto request);
        AppResponse<SupplierProductDto> EditSupplierProduct(SupplierProductDto request);
        AppResponse<string> DeleteSupplierProduct(Guid Id);

    }
}