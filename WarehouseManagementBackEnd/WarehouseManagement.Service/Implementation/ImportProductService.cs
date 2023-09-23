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
        private ISupplierProductRepository _supplierProductRepository;
        private IHttpContextAccessor _httpContextAccessor;

        public ImportProductService(IImportProductRepository importProductRepository, IMapper mapper,
            IInboundReceiptRepository inboundReceiptRepository, ISupplierProductRepository supplierProductRepository, IHttpContextAccessor httpContextAccessor)
        {
            _importProductRepository = importProductRepository;
            _mapper = mapper;
            _inboundReceiptRepository = inboundReceiptRepository;
            _supplierProductRepository = supplierProductRepository;
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
                if (requets.SupplierProductId == null)
                {
                    return result.BuildError("Supplier product cannot null");
                }
                var supplierProduct =_supplierProductRepository.FindBy(x=>x.Id == requets.SupplierProductId && x.IsDeleted == false);
                if(supplierProduct.Count() == 0)
                {
                    return result.BuildError("Cannot find supplier product");
                }
                if(requets.InboundReceiptId == null)
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
                importProduct.SupplierProduct = null;
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
                    .Include(x=>x.SupplierProduct)
                    .Include(x=>x.SupplierProduct.Supplier)
                    .Include(x=>x.SupplierProduct.Product);
                var list = query.Select(x=> new ImportProductDto
                {
                    Quantity = x.Quantity,
                    Id = x.Id,
                    InboundReceiptId = x.InboundReceiptId,
                    ProductName = x.SupplierProduct.Product.Name,
                    SipplierName = x.SupplierProduct.Supplier.Name,
                    SupplierProductId = x.SupplierProductId
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
                var importProduct = _importProductRepository.Get(Id);
                var data = _mapper.Map<ImportProductDto>(importProduct);
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
