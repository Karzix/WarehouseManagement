using System.Data.Entity;
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
    public class OutboundReceiptService:IOutboundReceiptService
    {
        private IOutboundReceiptRepository _outboundReceiptRepository;
        private IMapper _mapper;
        private IWarehouseRepository _warehouseRepository;
        private IHttpContextAccessor _httpContextAccessor;

        public OutboundReceiptService(IOutboundReceiptRepository outboundReceiptRepository,
            IMapper mapper, IWarehouseRepository warehouseRepository, IHttpContextAccessor httpContextAccessor)
        {
            _outboundReceiptRepository = outboundReceiptRepository;
            _mapper = mapper;
            _warehouseRepository = warehouseRepository;
            _httpContextAccessor = httpContextAccessor;
        }

        public AppResponse<OutboundReceiptDto> CreateOutboundReceipt(OutboundReceiptDto request)
        {
            var result = new AppResponse<OutboundReceiptDto>();
            try
            {
                var UserName = ClaimHelper.GetClainByName(_httpContextAccessor, "UserName");
                if (UserName == null)
                {
                    return result.BuildError("Cannot find Account by this user");
                }
                if (request.WarehouseId == null)
                {
                    return result.BuildError("warehouse cannot be null");
                }
                var warehouse =_warehouseRepository.FindBy(x=>x.Id == request.WarehouseId && x.IsDeleted ==false);
                if(warehouse.Count() == 0)
                {
                    return result.BuildError("Cannot find warehouse");
                }
                    var outboundReceipt = _mapper.Map<OutboundReceipt>(request);
                    outboundReceipt.Warehouse = null;
                    outboundReceipt.Id = Guid.NewGuid();
                    _outboundReceiptRepository.Add(outboundReceipt);

                    request.Id = outboundReceipt.Id;
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

        public AppResponse<string> DeleteOutboundReceipt(Guid Id)
        {
            var result = new AppResponse<string>();
            try
            {
                var outboundReceipt = _outboundReceiptRepository.Get(Id);
                outboundReceipt.IsDeleted = true;

                _outboundReceiptRepository.Edit(outboundReceipt);

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

        public AppResponse<OutboundReceiptDto> EditOutbountReceipt(OutboundReceiptDto request)
        {
            var result = new AppResponse<OutboundReceiptDto>();
            try
            {
                var outboundReceipt = new OutboundReceipt();
                outboundReceipt.Id = (Guid)request.Id;
                outboundReceipt.WarehouseId = request.WarehouseId;
                outboundReceipt.To = request.To;

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

        public AppResponse<List<OutboundReceiptDto>> GetAllOutboundReceipt()
        {
            var result = new AppResponse<List<OutboundReceiptDto>>();
            try
            {
                var query = _outboundReceiptRepository.GetAll()
                    .Include(x => x.Warehouse);
                var list = query.Select(m => new OutboundReceiptDto
                {
                    Id = m.Id,
                    To = m.To,
                    WarehouseName = m.Warehouse.Name,
                    WarehouseId = m.Id,

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

        public AppResponse<OutboundReceiptDto> GetOutboundReceipt(Guid Id)
        {
            var result = new AppResponse<OutboundReceiptDto>();
            try
            {
                var outboundReceipt = _outboundReceiptRepository.FindById(Id);
                var data = new OutboundReceiptDto();
                data.Id = outboundReceipt.Id;
                data.To = outboundReceipt.To;
                data.WarehouseId = outboundReceipt.WarehouseId;
                data.WarehouseName = outboundReceipt.Warehouse.Name;

                result.IsSuccess = true;
                result.Data = data;
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
