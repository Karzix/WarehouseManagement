using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MayNghien.Models.Request.Base;
using MayNghien.Models.Response.Base;
using WarehouseManagement.Model.Dto;

namespace WarehouseManagement.Service.Contract
{
    public interface IInboundReceiptService
    {
        AppResponse<List<InboundReceiptDto>> GetAllInboundReceipt();
        AppResponse<InboundReceiptDto> GetInboundReceipt(int Id);
        AppResponse<InboundReceiptDto> CreateInboundReceipt(InboundReceiptDto request);
        AppResponse<InboundReceiptDto> EditInboundReceipt(InboundReceiptDto request);
        AppResponse<string> DeleteInboundReceipt(int Id);
        AppResponse<SearchResponse<InboundReceiptDto>> Search(SearchRequest request);

        void ExportRecordtoExcel(SearchRequest request);

	}
}
