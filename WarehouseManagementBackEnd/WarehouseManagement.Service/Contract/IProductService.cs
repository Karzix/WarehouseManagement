﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MayNghien.Models.Request.Base;
using MayNghien.Models.Response.Base;
using WarehouseManagement.Model.Dto;

namespace WarehouseManagement.Service.Contract
{
    public interface IProductService
    {
        AppResponse<List<ProductDto>> GetAllProduct();
        AppResponse<ProductDto> GetProduct(int Id);
        AppResponse<ProductDto> CreateProduct(ProductDto request);
        AppResponse<ProductDto> EditProduct(ProductDto request);
        AppResponse<string> DeleteProduct(int Id);
        AppResponse<SearchResponse<ProductDto>> Search(SearchRequest request);
    }
}
