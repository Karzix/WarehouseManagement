using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MayNghien.Models.Response.Base;
using WarehouseManagement.Model.Dto;

namespace WarehouseManagement.Service.Contract
{
    public interface IOutboundReceiptService
    {
        AppResponse<List<OutboundReceiptDto>> GetAllOutboundReceipt();
        AppResponse<OutboundReceiptDto> GetOutboundReceipt(Guid Id);
        AppResponse<OutboundReceiptDto> CreateOutboundReceipt(OutboundReceiptDto request);
        AppResponse<OutboundReceiptDto> EditOutbountReceipt(OutboundReceiptDto request);
        AppResponse<string> DeleteOutboundReceipt(Guid Id);
    }
}
