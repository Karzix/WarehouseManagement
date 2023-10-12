using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MayNghien.Models.Request.Base;
using MayNghien.Models.Response.Base;
using WarehouseManagement.DAL.Models.Entity;
using WarehouseManagement.Model.Dto;

namespace WarehouseManagement.Service.Contract
{
    public interface IProductRemainingService
    {
        AppResponse<ProductRemainingDto> GetProductRemaining(int Id);
        AppResponse<List<ProductRemainingDto>> GetAllProductRemaining();
        AppResponse<ProductRemainingDto> CreateProductRemaining(ProductRemainingDto request);
        AppResponse<ProductRemainingDto> EditProductRemaining(ProductRemainingDto request);
        AppResponse<string> DeleteProductRemaining(int Id);
        AppResponse<SearchResponse<ProductRemainingDto>> Search(SearchRequest request);

	}
}
