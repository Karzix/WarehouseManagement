using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MayNghien.Models.Request.Base;
using MayNghien.Models.Response.Base;
using WarehouseManagement.Model.Dto;

namespace WarehouseManagement.Service.Contract
{
    public interface ISupplierService
    {
        AppResponse<SupplierDto> CreateSupplier(SupplierDto supplier);
        AppResponse<SupplierDto> EditSupplier(SupplierDto supplier);
        AppResponse<string> DeleteSupplier(int Id);
        AppResponse<List<SupplierDto>> GetAllSupplier();
        AppResponse<SupplierDto> GetSupplier(int Id);
        AppResponse<SearchResponse<SupplierDto>> Search(SearchRequest request);

	}
}
