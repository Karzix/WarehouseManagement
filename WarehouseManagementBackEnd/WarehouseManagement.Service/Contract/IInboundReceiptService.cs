using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MayNghien.Models.Response.Base;
using WarehouseManagement.Model.Dto;

namespace WarehouseManagement.Service.Contract
{
    public interface IInboundReceiptService
    {
        AppResponse<List<InboundReceiptDto>> GetAllInboundReceipt();
        AppResponse<InboundReceiptDto> GetInboundReceipt(Guid Id);
        AppResponse<InboundReceiptDto> CreateInboundReceipt(InboundReceiptDto request);
        AppResponse<InboundReceiptDto> EditInboundReceipt(InboundReceiptDto request);
        AppResponse<string> DeleteInboundReceipt(Guid Id);
    }
}
