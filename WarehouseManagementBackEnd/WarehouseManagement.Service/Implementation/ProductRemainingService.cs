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
    public class ProductRemainingService: IProductRemainingService
    {
        private readonly IProductRemainingRepository _productRemainingRepository;
        private IMapper _mapper;
        private IProductRepository _productRepository;
        private IWarehouseRepository _warehouseRepository;
        private IHttpContextAccessor _httpContextAccessor;

        public ProductRemainingService(IProductRemainingRepository productRemainingRepository,
            IMapper mapper, IProductRepository productRepository, IWarehouseRepository warehouseRepository, IHttpContextAccessor httpContextAccessor)
        {
            _productRemainingRepository = productRemainingRepository;
            _mapper = mapper;
            _productRepository = productRepository;
            _warehouseRepository = warehouseRepository;
            _httpContextAccessor = httpContextAccessor;
        }

        public AppResponse<ProductRemainingDto> CreateProductRemaining(ProductRemainingDto request)
        {
            var result = new AppResponse<ProductRemainingDto>();
            try
            {
                var UserName = ClaimHelper.GetClainByName(_httpContextAccessor, "UserName");
                if (UserName == null)
                {
                    return result.BuildError("Cannot find Account by this user");
                }
                if (request.ProductId == null)
                {
                    return result.BuildError("product cannot null");
                }
                var product = _productRepository.FindBy(x=>x.Id == request.ProductId && x.IsDeleted == false);
                if (product.Count() == 0)
                {
                    return result.BuildError("Cannot find product");
                }
                if(request.WarehouseId== null)
                {
                    return result.BuildError("warehouse cannot null");
                }
                var warehouse =_warehouseRepository.FindBy(x=>x.Id == request.WarehouseId && x.IsDeleted == false);
                if(warehouse.Count() == 0)
                {
                    return result.BuildError("Cannot find warehouse");
                }
                    var productREmaining = _mapper.Map<ProductRemaining>(request);
                    productREmaining.Id = Guid.NewGuid();
                    productREmaining.Product = null;
                    productREmaining.Warehouse = null;
                    productREmaining.CreatedBy = UserName;
                    _productRemainingRepository.Add(productREmaining);
                    request.Id = productREmaining.Id;
                    result.IsSuccess = true;
                    result.Data = request;
            }
            catch (Exception ex)
            {
                result.IsSuccess = false;
                result.Message = ex.Message+ " " + ex.StackTrace;
            }
            return result;
        }

        public AppResponse<string> DeleteProductRemaining(Guid Id)
        {
            var result = new AppResponse<string>();
            try
            {
                var productRemaining = _productRemainingRepository.Get(Id);
                productRemaining.IsDeleted = true;
                _productRemainingRepository.Edit(productRemaining);
                result.IsSuccess = true;
                result.Data = "đã xóa";
            }
            catch (Exception ex)
            {
                result.IsSuccess = false;
                result.Message = ex.Message + " " + ex.StackTrace;
            }
            return result;
        }

        public AppResponse<ProductRemainingDto> EditProductRemaining(ProductRemainingDto request)
        {
            var result = new AppResponse<ProductRemainingDto>();
            try
            {
                var productRemaining = _productRemainingRepository.Get(request.Id.Value);
                productRemaining.ProductId = request.ProductId;
                productRemaining.Quantity = request.Quantity;
                productRemaining.WarehouseId = request.WarehouseId;
                productRemaining.ModifiedOn = DateTime.UtcNow;
                productRemaining.Id = (Guid)request.Id;

                result.IsSuccess = true;
                result.Data = request;


            }
            catch (Exception ex)
            {
                result.IsSuccess = false;
                result.Message = ex.Message + " " + ex.StackTrace;
            }
            return result;
        }

        public AppResponse<List<ProductRemainingDto>> GetAllProductRemaining()
        {
            var result = new AppResponse<List<ProductRemainingDto>>();
            try
            {
                var query = _productRemainingRepository.GetAll()
                    .Include(x => x.Product)
                    .Include(x => x.Warehouse);
                var list = query.Select(m => new ProductRemainingDto
                {
                    Id = m.Id,
                    WarehouseId = m.WarehouseId,
                    WarehouseName = m.Warehouse.Name,
                    ProductId = m.ProductId,
                    ProductName = m.Product.Name,
                    Quantity = m.Quantity,
                })
                .ToList();

                result.Data = list;
                result.IsSuccess = true;
                return result;
            }
            catch (Exception ex)
            {
                result.IsSuccess = false;
                result.Message = ex.Message + " " + ex.StackTrace;
            }
            return result;
        }

        public AppResponse<ProductRemainingDto> GetProductRemaining(Guid Id)
        {
            var result = new AppResponse<ProductRemainingDto>();
            try
            {
                var productRemaining = _productRemainingRepository.FindById(Id);

                var data = _mapper.Map<ProductRemainingDto>(productRemaining);

                result.Data = data;
                result.IsSuccess = true;
            }
            catch (Exception ex)
            {
                result.IsSuccess = false;
                result.Message = ex.Message + " " + ex.StackTrace;
            }
            return result;
        }
    }
}
