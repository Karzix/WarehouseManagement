﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MayNghien.Models.Request.Base;
using MayNghien.Models.Response.Base;
using WarehouseManagement.Model.Dto;

namespace WarehouseManagement.Service.Contract
{
    public interface IOutboundReceiptService
    {
        AppResponse<List<OutboundReceiptDto>> GetAllOutboundReceipt();
        AppResponse<OutboundReceiptDto> GetOutboundReceipt(int Id);
        AppResponse<OutboundReceiptDto> CreateOutboundReceipt(OutboundReceiptDto request);
        AppResponse<OutboundReceiptDto> EditOutbountReceipt(OutboundReceiptDto request);
        AppResponse<string> DeleteOutboundReceipt(int Id);
        AppResponse<SearchResponse<OutboundReceiptDto>> Search(SearchRequest request);

	}
}
