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
        public ExportProductService(IExportProductRepository repository, IMapper mapper)
        {
            _repository = repository;
            this._mapper = mapper;
        }

        public AppResponse<ExportProductDto> CreateExportProduct(ExportProductDto request)
        {
            var result = new AppResponse<ExportProductDto>();
            try
            {
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
                    .Include(s=> s.SupplierProduct.Supplier);
                var list = query.Select(m => new ExportProductDto
                {
                    Quantity = m.Quantity,
                    SupplierProductId = m.SupplierProductId,
                    OutboundReceiptId = m.OutboundReceiptId,
                    SupplierProductName = m.SupplierProduct.Supplier.Name,
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

        public AppResponse<ExportProductDto> GetExportProduct(Guid Id)
        {
            var result = new AppResponse<ExportProductDto>();
            try
            {
                var exportProduct = _repository.FindById(Id);
                var exportProductDto = _mapper.Map<ExportProductDto>(exportProduct);
                exportProductDto.SupplierProductName = exportProduct.SupplierProduct.Supplier.Name;

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
