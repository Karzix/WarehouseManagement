using MayNghien.Models.Request.Base;
using MayNghien.Models.Response.Base;
using WarehouseManagement.DAL.Models.Entity;
using WarehouseManagement.Model.Dto;

namespace WarehouseManagement.Service.Contract
{
    public interface IWarehouseService
    {
        AppResponse<List<WarehouseDto>> GetAllWarehouse();
        AppResponse<WarehouseDto> GetWarehouseById(Guid Id);
        AppResponse<WarehouseDto> CreateWarehouse(WarehouseDto request);
        AppResponse<WarehouseDto> EditWarehouse(WarehouseDto request);
        AppResponse<string> DeleteWarehouse(Guid Id);
        AppResponse<SearchResponse<WarehouseDto>> Search(SearchRequest request);

	}
}
