using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MayNghien.Models.Request.Base;
using MayNghien.Models.Response.Base;
using WarehouseManagement.Model.Dto;
using WarehouseManagement.Model.Response.User;

namespace WarehouseManagement.Service.Contract
{
    public interface ISupplierProductService
    {
        AppResponse<List<SupplierProductDto>> GetAllSupplierProduct();
        AppResponse<SupplierProductDto> GetSupplierProduct(int Id);
        AppResponse<SupplierProductDto> CreateSupplierProduct(SupplierProductDto request);
        AppResponse<SupplierProductDto> EditSupplierProduct(SupplierProductDto request);
        AppResponse<string> DeleteSupplierProduct(int Id);
        AppResponse<SearchResponse<SupplierProductDto>> Search(SearchRequest request);

    }
}
