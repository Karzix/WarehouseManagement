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
    public class OutboundReceiptService:IOutboundReceiptService
    {
        private IOutboundReceiptRepository _outboundReceiptRepository;
        private IMapper _mapper;

        public OutboundReceiptService(IOutboundReceiptRepository outboundReceiptRepository, IMapper mapper)
        {
            _outboundReceiptRepository = outboundReceiptRepository;
            _mapper = mapper;
        }

        public AppResponse<OutboundReceiptDto> CreateOutboundReceipt(OutboundReceiptDto request)
        {
            var result = new AppResponse<OutboundReceiptDto>();
            try
            {
                var outboundReceipt = _mapper.Map<OutboundReceipt>(request);
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
