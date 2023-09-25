﻿using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using MayNghien.Common.Helpers;
using MayNghien.Models.Response.Base;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging.Abstractions;
using WarehouseManagement.DAL.Contract;
using WarehouseManagement.DAL.Models.Entity;
using WarehouseManagement.Model.Dto;
using WarehouseManagement.Service.Contract;

namespace WarehouseManagement.Service.Implementation
{
    public class ImportProductService:IImportProductService
    {
        private IImportProductRepository _importProductRepository;
        private IMapper _mapper;
        private IInboundReceiptRepository _inboundReceiptRepository;
        private ISupplierRepository _supplierRepository;
        private IProductRepository _productRepository;
        private IHttpContextAccessor _httpContextAccessor;

        public ImportProductService(IImportProductRepository importProductRepository, IMapper mapper, IInboundReceiptRepository inboundReceiptRepository, ISupplierRepository supplierRepository, IProductRepository productRepository, IHttpContextAccessor httpContextAccessor)
        {
            _importProductRepository = importProductRepository;
            _mapper = mapper;
            _inboundReceiptRepository = inboundReceiptRepository;
            _supplierRepository = supplierRepository;
            _productRepository = productRepository;
            _httpContextAccessor = httpContextAccessor;
        }

        public AppResponse<ImportProductDto> CreateImportProduct(ImportProductDto requets)
        {
            var result = new AppResponse<ImportProductDto>();
            try
            {
                var UserName = ClaimHelper.GetClainByName(_httpContextAccessor, "UserName");
                if (UserName == null)
                {
                    return result.BuildError("Cannot find Account by this user");
                }
                if (requets.SupplierId == null)
                {
                    return result.BuildError("Supplier cannot null");
                }
                var supplier =_supplierRepository.FindBy(x=>x.Id == requets.SupplierId && x.IsDeleted == false);
                if(supplier.Count() == 0)
                {
                    return result.BuildError("Cannot find supplier");
                }
                if (requets.ProductId == null)
                {
                    return result.BuildError("Product cannot null");
                }
                var product = _productRepository.FindBy(x => x.Id == requets.SupplierId && x.IsDeleted == false);
                if (product.Count() == 0)
                {
                    return result.BuildError("Cannot find product");
                }
                if (requets.InboundReceiptId == null)
                {
                    return result.BuildError("Inbound receipt cannot null");
                }
                var inboundReceipt =_inboundReceiptRepository.FindBy(x=>x.Id == requets.InboundReceiptId && x.IsDeleted == false);
                if(inboundReceipt.Count() == 0)
                {
                    return result.BuildError("Cannot find inbound receipt");
                }
                var importProduct = _mapper.Map<ImportProduct>(requets);
                importProduct.Id = Guid.NewGuid();
                importProduct.Supplier = null;
                importProduct.Product = null;
                importProduct.InboundReceipt = null;
                importProduct.CreatedBy = UserName;
                _importProductRepository.Add(importProduct);

                requets.Id = importProduct.Id;
                result.BuildResult(requets);
            }
            catch(Exception ex)
            {
                result.BuildError(ex.Message);
            }
            return result;
        }

        public AppResponse<string> DeleteImportProduct(Guid Id)
        {
            var result =  new AppResponse<string>();
            try
            {
                var importProduct = _importProductRepository.Get(Id);
                importProduct.IsDeleted = true;
                _importProductRepository.Edit(importProduct);
                result.BuildResult("đã xóa");
            }
            catch (Exception ex)
            {
                result.BuildError(ex.Message);
            }
            return result;
        }

        public AppResponse<ImportProductDto> EditImportProduct(ImportProductDto requets)
        {
            var result = new AppResponse<ImportProductDto>();
            try
            {
                var inportProduct = _importProductRepository.Get(requets.Id.Value);
                inportProduct.SupplierId = requets.SupplierId;
                inportProduct.ProductId = requets.ProductId;
                inportProduct.Quantity = requets.Quantity;
                inportProduct.InboundReceiptId = requets.InboundReceiptId;
                inportProduct.ModifiedOn = DateTime.UtcNow;
            }
            catch (Exception ex)
            {
                result.BuildError(ex.Message);
            }
            return result;
        }

        public AppResponse<List<ImportProductDto>> GetAllImportProduct()
        {
            var result = new AppResponse<List<ImportProductDto>>();
            try
            {
                var query = _importProductRepository.GetAll()
                    .Include(x => x.Supplier)
                    .Include(x => x.Product);
                var list = query.Select(x=> new ImportProductDto
                {
                    Quantity = x.Quantity,
                    Id = x.Id,
                    InboundReceiptId = x.InboundReceiptId,
                    ProductName = x.Product.Name,
                    SipplierName = x.Supplier.Name,
                    SupplierId = x.SupplierId,
                    ProductId = x.ProductId,
                }).ToList();

                result.BuildResult(list);
            }
            catch (Exception ex)
            {
                result.BuildError(ex.Message);
            }
            return result;
        }

        public AppResponse<ImportProductDto> GetImportProduct(Guid Id)
        {
            var result = new AppResponse<ImportProductDto>();
            try
            {
                var query = _importProductRepository.FindBy(x=>x.Id == Id)
                    .Include(x=>x.Supplier)
                    .Include(x=>x.Product);
                var data =  query.Select(x=> new ImportProductDto
                {
                    Quantity = x.Quantity,
                    Id = x.Id,
                    InboundReceiptId = x.InboundReceiptId,
                    ProductId = x.ProductId,
                    ProductName = x.Product.Name,
                    SipplierName = x.Supplier.Name,
                    SupplierId = x.SupplierId,
                }).First();
                result.BuildResult(data);
            }
            catch (Exception ex)
            {
                result.BuildError(ex.Message);
            }
            return result;
        }
    }
}
