using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using MayNghien.Models.Response.Base;
using WarehouseManagement.DAL.Contract;
using WarehouseManagement.DAL.Models.Entity;
using WarehouseManagement.Model.Dto;
using WarehouseManagement.Service.Contract;

namespace WarehouseManagement.Service.Implementation
{
    public class InboundReceiptService : IInboundReceiptService
    {
        private readonly IInboundReceiptRepository _inboundReceiptRepository;
        private IMapper _mapper;
        private ISupplierRepository _supplierRepository;
        private IWarehouseRepository _warehouseRepository;

        public InboundReceiptService(IInboundReceiptRepository inboundReceiptRepository, IMapper mapper,
            ISupplierRepository supplierRepository, IWarehouseRepository warehouseRepository)
        {
            _inboundReceiptRepository = inboundReceiptRepository;
            _mapper = mapper;
            _supplierRepository = supplierRepository;
            _warehouseRepository = warehouseRepository;
        }

        public AppResponse<InboundReceiptDto> CreateInboundReceipt(InboundReceiptDto request)
        {
            var result = new AppResponse<InboundReceiptDto>();
            try
            {
                if (request.SupplierId == null)
                {
                    return result.BuildError("supplier cannot be null");
                }
                else if (request.WarehouseId == null)
                {
                    return result.BuildError("warehouse cannot be null");
                }
                var supplier =_supplierRepository.FindBy(x=>x.Id ==  request.SupplierId && x.IsDeleted == false);
                if (supplier.Count() == 0)
                {
                    return result.BuildError("Cannot find supplier");
                }
                var warehouse = _warehouseRepository.FindBy(x=>x.Id == request.WarehouseId && x.IsDeleted == false);
                if(warehouse.Count() == 0)
                {
                    return result.BuildError("Cannot find warehouse");
                }
                var inboundReceipt = _mapper.Map<InboundReceipt>(request);
                    inboundReceipt.Id = Guid.NewGuid();
                    inboundReceipt.Warehouse = null;
                    inboundReceipt.Supplier = null;
                    _inboundReceiptRepository.Add(inboundReceipt);
                    request.Id = inboundReceipt.Id;
                    result.BuildResult(request);    
            }
            catch (Exception ex)
            {
                result.BuildError(ex.Message + "" + ex.StackTrace);
            }
            return result;
        }

        public AppResponse<string> DeleteInboundReceipt(Guid Id)
        {
            var result = new AppResponse<string>();
            try
            {
                var inboundReceipt =_inboundReceiptRepository.Get(Id);
                inboundReceipt.IsDeleted = true;
                _inboundReceiptRepository.Edit(inboundReceipt);
                result.BuildResult("đã xóa");
            }
            catch (Exception ex)
            {
                result.BuildError(ex.Message + "" + ex.StackTrace);
            }
            return result;
        }

        public AppResponse<InboundReceiptDto> EditInboundReceipt(InboundReceiptDto request)
        {
            var result = new AppResponse<InboundReceiptDto>();
            try
            {
                var inboundReceipt = _mapper.Map<InboundReceipt>(request);
                _inboundReceiptRepository.Edit(inboundReceipt);
                result.BuildResult(request);
            }
            catch (Exception ex)
            {
                result.BuildError(ex.Message + "" + ex.StackTrace);
            }
            return result;
        }

        public AppResponse<List<InboundReceiptDto>> GetAllInboundReceipt()
        {
            var result = new AppResponse<List<InboundReceiptDto>>();
            try
            {
                var query = _inboundReceiptRepository.GetAll()
                    .Include(x=>x.Supplier)
                    .Include(x=>x.Supplier);
                var list = query.Select(x=> new InboundReceiptDto
                    {
                        Id = x.Id,
                        SupplierId = x.SupplierId,
                        SupplierName = x.Supplier.Name,
                        WarehouseId = x.WarehouseId,
                        WarehousName = x.Warehouse.Name
                    }).ToList();

                result.BuildResult(list);
            }
            catch (Exception ex)
            {
                result.BuildError(ex.Message + "" + ex.StackTrace);
            }
            return result;
        }

        public AppResponse<InboundReceiptDto> GetInboundReceipt(Guid Id)
        {
            var result = new AppResponse<InboundReceiptDto>();
            try
            {
                var inboundReceipt = _inboundReceiptRepository.FindBy(x=>x.Id == Id).First();
                var data = _mapper.Map<InboundReceiptDto>(inboundReceipt);
                result.BuildResult(data);
            }
            catch (Exception ex)
            {
                result.BuildError(ex.Message + "" + ex.StackTrace);
            }
            return result;
        }
    }
}
