﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using MayNghien.Common.Helpers;
using MayNghien.Models.Response.Base;
using Microsoft.AspNetCore.Http;
using WarehouseManagement.DAL.Contract;
using WarehouseManagement.DAL.Models.Entity;
using WarehouseManagement.Model.Dto;
using WarehouseManagement.Service.Contract;

namespace WarehouseManagement.Service.Implementation
{
    public class ProductService : IProductService
    {
        private readonly IProductRepository _productRepository;
        private readonly IMapper _mapper;
        private IHttpContextAccessor _httpContextAccessor;

        public ProductService(IProductRepository productRepository, IMapper mapper, IHttpContextAccessor httpContextAccessor)
        {
            _productRepository = productRepository;
            _mapper = mapper;
            _httpContextAccessor = httpContextAccessor;
        }

        public AppResponse<ProductDto> CreateProduct(ProductDto request)
        {
            var result = new AppResponse<ProductDto>();
            try
            {
                var UserName = ClaimHelper.GetClainByName(_httpContextAccessor, "UserName");
                if (UserName == null)
                {
                    return result.BuildError("Cannot find Account by this user");
                }
                var product = new Product();
                product = _mapper.Map<Product>(request);
                product.Id = Guid.NewGuid();
                product.CreatedBy = UserName;
                _productRepository.Add(product);

                result.IsSuccess = true;
                result.Data = request;
                return result;
            }
            catch(Exception ex)
            {
                result.IsSuccess = false;
                result.Message = ex.Message + " " + ex.StackTrace;
                return result;
            }
        }

        public AppResponse<string> DeleteProduct(Guid Id)
        {
            var result = new AppResponse<string>();
            try
            {
                var product = _productRepository.Get(Id);

                product.IsDeleted = true;   
                _productRepository.Edit(product);
                result.IsSuccess = true;
                result.Data = "Đã xóa";
                return result;
            }
            catch(Exception ex)
            {
                result.IsSuccess = false;
                result.Message = ex.Message + " " + ex.StackTrace;
                return result;
            }
        }

        public AppResponse<List<ProductDto>> GetAllProduct()
        {
            var result = new AppResponse<List<ProductDto>>();
            try
            {
                var query = _productRepository.GetAll();
                var list = query.Select(m => new ProductDto
                {
                    Name = m.Name,
                    Description = m.Description,
                    Quantity = m.Quantity,
                    Id = m.Id
                }).ToList();

                result.Data = list;
                result.IsSuccess = true;
                return result;
            }
            catch(Exception ex)
            {
                result.IsSuccess = false;
                result.Message = ex.Message + " " + ex.StackTrace;
                return result;
            }
        }

        public AppResponse<ProductDto> GetProduct(Guid Id)
        {
            var result = new AppResponse<ProductDto>();
            try
            {
                var product = _productRepository.Get(Id);

                result.Data = _mapper.Map<ProductDto>(product);
                result.IsSuccess = true;
                return result;
            }
            catch (Exception ex)
            {
                result.IsSuccess = false;
                result.Message = ex.Message + " " + ex.StackTrace;
                return result;
            }
        }

        public AppResponse<ProductDto> EditProduct(ProductDto request)
        {
            var result = new AppResponse<ProductDto>();
            try
            {
                var product = _productRepository.Get(request.Id.Value);
                product.Description = request.Description;
                product.Quantity = request.Quantity;
                product.Description = request.Description;
                product.ModifiedOn = DateTime.UtcNow;
                _productRepository.Edit(product);
                result.Data = request;
                result.IsSuccess = true;
                return result;
            }
            catch (Exception ex)
            {
                result.IsSuccess = false;
                result.Message = ex.Message + " " + ex.StackTrace;
                return result;
            }
        }
    }
}
