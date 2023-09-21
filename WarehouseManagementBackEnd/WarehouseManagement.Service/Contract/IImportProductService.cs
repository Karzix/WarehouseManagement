using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MayNghien.Models.Response.Base;
using WarehouseManagement.Model.Dto;

namespace WarehouseManagement.Service.Contract
{
    public interface IImportProductService
    {
        AppResponse<List<ImportProductDto>> GetAllImportProduct();
        AppResponse<ImportProductDto> GetImportProduct(Guid Id);
        AppResponse<ImportProductDto> CreateImportProduct(ImportProductDto requets);
        AppResponse<ImportProductDto> EditImportProduct(ImportProductDto requets);
        AppResponse<string> DeleteImportProduct(Guid Id);
    }
}
