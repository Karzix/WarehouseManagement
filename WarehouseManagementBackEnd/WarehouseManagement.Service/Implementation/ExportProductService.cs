using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using MayNghien.Models.Response.Base;
using WarehouseManagement.DAL.Contract;
using WarehouseManagement.DAL.Implementation;
using WarehouseManagement.DAL.Models.Entity;
using WarehouseManagement.Model.Dto;
using WarehouseManagement.Service.Contract;

namespace WarehouseManagement.Service.Implementation
{
    public class ExportProductService : IExportProductService
    {
        private readonly IExportProductRepository _repository;
        private readonly IMapper _mapper;
        private IOutboundReceiptRepository _outboundReceiptRepository;
        private ISupplierProductRepository _supplierProductRepository;
        public ExportProductService(IExportProductRepository repository, IMapper mapper
            , IOutboundReceiptRepository outboundReceiptRepository, ISupplierProductRepository supplierProductRepository)
        {
            _repository = repository;
            this._mapper = mapper;
            _outboundReceiptRepository = outboundReceiptRepository;
            _supplierProductRepository = supplierProductRepository;
        }

        public AppResponse<ExportProductDto> CreateExportProduct(ExportProductDto request)
        {
            var result = new AppResponse<ExportProductDto>();
            try
            {
                if(request.SupplierProductId == null)
                {
                    return result.BuildError("SupplierProduct cannot null");
                }
                var supplierProduct =_supplierProductRepository.FindBy(x=>x.Id == request.SupplierProductId && x.IsDeleted == false);
                if (supplierProduct.Count() == 0)
                {
                    return result.BuildError("Cannot find supplier product");
                }
                if(request.OutboundReceiptId == null)
                {
                    return result.BuildError("Outbound receipt cannot null");
                }
                var outboundReceipt =_outboundReceiptRepository.FindBy(x=>x.Id ==request.OutboundReceiptId && x.IsDeleted==false);
                if(outboundReceipt.Count() == 0)
                {
                    return result.BuildError("Cannot find outbound receipt");
                }
                var exportProduct = _mapper.Map<ExportProduct>(request);
                exportProduct.Id = Guid.NewGuid();
                _repository.Add(exportProduct);

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

        public AppResponse<string> DeleteExportProduct(Guid Id)
        {
            var result = new AppResponse<string>();
            try
            {
                var exportProduct = _repository.Get(Id);

                exportProduct.IsDeleted = true;
                _repository.Edit(exportProduct);
                result.IsSuccess = true;
                result.Data = "Đã xóa";
                return result;
            }
            catch (Exception ex)
            {
                result.IsSuccess = false;
                result.Message = ex.Message + " " + ex.StackTrace;
                return result;
            }
        }

        public AppResponse<ExportProductDto> EditExportProduct(ExportProductDto request)
        {
            var result = new AppResponse<ExportProductDto>();
            try
            {
                var exportProduct = new ExportProduct();
                exportProduct.Id = (Guid)request.Id;
                exportProduct.SupplierProductId = request.SupplierProductId;
                exportProduct.OutboundReceiptId = request.OutboundReceiptId;
                exportProduct.Quantity = request.Quantity;

                _repository.Edit(exportProduct);
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

        public AppResponse<List<ExportProductDto>> GetAllExportProduct()
        {
            var result = new AppResponse<List<ExportProductDto>>();
            try
            {
                var query = _repository.GetAll()
                    .Include(s => s.SupplierProduct)
                    .Include(s=> s.SupplierProduct.Supplier)
                    .Include(s=>s.SupplierProduct.Product);
                var list = query.Select(m => new ExportProductDto
                {
                    Quantity = m.Quantity,
                    SupplierProductId = m.SupplierProductId,
                    OutboundReceiptId = m.OutboundReceiptId,
                    Id = m.Id,
                    SupplierName = m.SupplierProduct.Supplier.Name,
                    ProductName = m.SupplierProduct.Product.Name
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

        public AppResponse<ExportProductDto> GetExportProduct(Guid Id)
        {
            var result = new AppResponse<ExportProductDto>();
            try
            {
                var exportProduct = _repository.FindById(Id);
                var exportProductDto = _mapper.Map<ExportProductDto>(exportProduct);
                exportProductDto.SupplierName = exportProduct.SupplierProduct.Supplier.Name;
                exportProductDto.ProductName = exportProduct.SupplierProduct.Product.Name;

                result.IsSuccess = true;
                result.Data = exportProductDto;
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
