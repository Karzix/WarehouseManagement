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
    public interface IExportProductService
    {
        AppResponse<List<ExportProductDto>> GetAllExportProduct();
        AppResponse<ExportProductDto> GetExportProduct(Guid Id);
        AppResponse<ExportProductDto> EditExportProduct(ExportProductDto request);
        AppResponse<ExportProductDto> CreateExportProduct(ExportProductDto request);
        AppResponse<string> DeleteExportProduct(Guid Id);
        AppResponse<SearchResponse<ExportProductDto>> Search(SearchRequest request);

	}
}
