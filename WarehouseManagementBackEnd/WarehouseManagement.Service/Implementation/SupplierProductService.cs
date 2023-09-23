using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using MayNghien.Common.Helpers;
using MayNghien.Models.Response.Base;
using Microsoft.AspNetCore.Http;
using WarehouseManagement.DAL.Contract;
using WarehouseManagement.DAL.Implementation;
using WarehouseManagement.DAL.Models.Entity;
using WarehouseManagement.Model.Dto;
using WarehouseManagement.Service.Contract;

namespace WarehouseManagement.Service.Implementation
{
    public class SupplierProductService : ISupplierProductService
    {
        private readonly ISupplierProductRepository _supplierProductRepository;
        private readonly IMapper _mapper;
        private ISupplierRepository _supplierRepository;
        private IProductRepository _productRepository;
        private IHttpContextAccessor _httpContextAccessor;
        public SupplierProductService(ISupplierProductRepository supplierProductRepository, IMapper mapper,
            ISupplierRepository supplierRepository, IProductRepository productRepository, IHttpContextAccessor httpContextAccessor)
        {
            _supplierProductRepository = supplierProductRepository;
            _mapper = mapper;
            _supplierRepository = supplierRepository;
            _productRepository = productRepository;
            _httpContextAccessor = httpContextAccessor;
        }

        public AppResponse<SupplierProductDto> CreateSupplierProduct(SupplierProductDto request)
        {
            var result = new AppResponse<SupplierProductDto>();
            try
            {
                var UserName = ClaimHelper.GetClainByName(_httpContextAccessor, "UserName");
                if (UserName == null)
                {
                    return result.BuildError("Cannot find Account by this user");
                }

                var supplier = _supplierRepository.Get(request.SupplierId);
                var product =_productRepository.Get(request.ProductId);
                if (supplier == null && supplier.IsDeleted != false)
                {

                    return result.BuildError("supplier wrong!");
                }
                else if (product == null && product.IsDeleted != false)
                {
                    return result.BuildError("product wrong!");
                }
                else
                {
                    var supplierproduct = _mapper.Map<SupplierProduct>(request);
                    supplierproduct.Id = Guid.NewGuid();
                    supplierproduct.Product = product;
                    supplierproduct.Supplier = supplier;
                    supplierproduct.CreatedBy = UserName;
                    _supplierProductRepository.Add(supplierproduct);

                    request.Id = supplierproduct.Id;
                    result.IsSuccess = true;
                    result.Data = request;
                    return result;
                }
            }
            catch (Exception ex)
            {
                result.IsSuccess = false;
                result.Message = ex.Message + " " + ex.StackTrace;
                return result;
            }
        }

        public AppResponse<string> DeleteSupplierProduct(Guid Id)
        {
            var result = new AppResponse<string>();
            try
            {
                var supplierProduct = _supplierProductRepository.Get(Id);
                supplierProduct.IsDeleted = true;
                result.IsSuccess = true;
                result.Data = "đã xóa";
                return result;
            }
            catch (Exception ex)
            {
                result.IsSuccess = false;
                result.Message = ex.Message + " " + ex.StackTrace;
                return result;
            }
        }

        public AppResponse<SupplierProductDto> EditSupplierProduct(SupplierProductDto request)
        {
            var result = new AppResponse<SupplierProductDto>();
            try
            {
                var supplierPoduct = new SupplierProduct
                {
                    Id = (Guid)request.Id,
                    ProductId = request.ProductId,
                    SupplierId = request.SupplierId,
                    ModifiedOn = DateTime.UtcNow
                };
                _supplierProductRepository.Edit(supplierPoduct);
                result.IsSuccess = true;
                result.Data = request;
                return result;
            }
            catch (Exception ex)
            {
                result.IsSuccess = false;
                result.Message = ex.Message + " " + ex.StackTrace;
                return result;
            }
        }

        public AppResponse<List<SupplierProductDto>> GetAllSupplierProduct()
        {
            var result = new AppResponse<List<SupplierProductDto>>();
            try
            {
                var query = _supplierProductRepository.GetAll()
                    .Include(x=>x.Supplier)
                    .Include(x=>x.Product);
                var list = query.Select(m => new SupplierProductDto
                {
                    SupplierId = m.SupplierId,
                    ProductId = m.ProductId,
                    SupplierName = m.Supplier.Name,
                    ProductName = m.Product.Name,
                    Id = m.Id
                }).ToList();

                result.Data = list;
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

        public AppResponse<SupplierProductDto> GetSupplierProduct(Guid Id)
        {
            var result = new AppResponse<SupplierProductDto>();
            try
            {
                var supplierProduct = _supplierProductRepository.FindById(Id);
                var data = new SupplierProductDto();
                data.SupplierName =supplierProduct.Supplier.Name;
                data.SupplierId = supplierProduct.SupplierId;
                data.ProductName = supplierProduct.Product.Name;
                data.ProductId =supplierProduct.ProductId;
                data.Id = supplierProduct.Id;
                result.Data = data;
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
