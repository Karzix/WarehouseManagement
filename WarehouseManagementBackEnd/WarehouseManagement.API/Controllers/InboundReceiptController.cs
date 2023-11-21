using System.Collections.Generic;
using System.Data.Entity.Core.Common.CommandTrees.ExpressionBuilder;
using MayNghien.Models.Request.Base;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using OfficeOpenXml;
using WarehouseManagement.DAL.Models.Entity;
using WarehouseManagement.Model.Dto;
using WarehouseManagement.Service.Contract;
using WarehouseManagement.Service.Implementation;

namespace WarehouseManagement.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(AuthenticationSchemes = "Bearer")]
    public class InboundReceiptController : Controller
    {
        private IInboundReceiptService _inboundReceiptService;
        private IImportProductService _importProductService;
        public InboundReceiptController(IInboundReceiptService inboundReceiptService, IImportProductService importProductService)
        {
            _inboundReceiptService = inboundReceiptService;
            _importProductService = importProductService;
        }
        [HttpGet]
        public IActionResult GetAllInboundReceipt()
        {
            var result = _inboundReceiptService.GetAllInboundReceipt();
            return Ok(result);
        }
        [HttpGet]
        [Route("{Id}")]
        public IActionResult GetInboundReceipt(int Id)
        {
            var result = _inboundReceiptService.GetInboundReceipt(Id);
            return Ok(result);
        }
        [HttpPost]
        public IActionResult CreateInboundReceipt(InboundReceiptDto request)
        {
            var  result =_inboundReceiptService.CreateInboundReceipt(request);
            return Ok(result);
        }
        [HttpPut]
        [Route("{Id}")]
        public IActionResult EditInboundReceipt(InboundReceiptDto request)
        {
            var result =_inboundReceiptService.EditInboundReceipt(request);
            return Ok(result);
        }
        [HttpDelete]
        [Route("{Id}")]
        public IActionResult DeleteInboundReceipt(int Id)
        {
            var result = _inboundReceiptService.DeleteInboundReceipt(Id);
            return Ok(result);
        }
		[HttpPost]
		[Route("search")]
		public IActionResult Search(SearchRequest request)
		{
			var result = _inboundReceiptService.Search(request);
			return Ok(result);
		}
       
		[HttpPost("Download")]
		public IActionResult DownloadSelectedRows(SearchRequest request)
		{
			request.PageSize = int.MaxValue;
			request.PageIndex = 1;
			var listInboundReceipt = new List<InboundReceiptDto>();
			listInboundReceipt	= _inboundReceiptService.Search(request).Data.Data;
			using (var package = new ExcelPackage())
			{
				var worksheet = package.Workbook.Worksheets.Add("SelectedRows");
				int row = 0;
				int countInboundReceipt = 0;
				var listWarehouseId = listInboundReceipt.Select(x=>x.WarehouseId).Distinct().ToList(); 
				for (int i = 0; i < listWarehouseId.Count; i++)
				{
					var listInboundReceiptForWarehouseID = listInboundReceipt.Where(x=>x.WarehouseId == listWarehouseId[i]).OrderBy(x=>x.CreatedOn).ToList();
					worksheet.Cells[row + 1, 1].Value = "Kho: " + listInboundReceiptForWarehouseID[0].WarehouseName;
                    worksheet.Cells[row + 2, 1].Value = "Ngày nhập";
                    worksheet.Cells[row + 2, 2].Value = "Nhà cung cấp";
                    worksheet.Cells[row + 2, 3].Value = "Sản phẩm";
                    worksheet.Cells[row + 2, 4].Value = "Số lượng";
                    var listDate = listInboundReceiptForWarehouseID.Select(x => x.CreatedOn.Value.ToString("dd/MM/yyyy")).Distinct().ToList();
                    for(int j = 0; j < listDate.Count; j++)
                    {
                        
                        var listImportProduct = new List<ImportProductDto>();
                        for(int z =0;z < listInboundReceiptForWarehouseID.Count; z++)
                        {
                            if (listInboundReceiptForWarehouseID[z].CreatedOn.Value.ToString("dd/MM/yyyy") == listDate[j])
                            {
								var searchImportProduct = new SearchRequest();
								searchImportProduct.Filters = new List<Filter>();
								searchImportProduct.PageSize = int.MaxValue;
								searchImportProduct.PageIndex = 1;
								searchImportProduct.Filters.Add(new Filter
								{
									FieldName = "Day",
									Value = listDate[j],
									Operation = ""
								});
								searchImportProduct.Filters.Add(new Filter
								{
									FieldName = "IsDelete",
									Value = "false",
									Operation = ""
								});
                                searchImportProduct.Filters.Add(new Filter
								{
									FieldName = "InboundReceiptId",
									Value = listInboundReceiptForWarehouseID[z].Id.ToString(),
									Operation = ""
								});
                                var resultImportProduct = _importProductService.Search(searchImportProduct).Data.Data;
								listImportProduct.AddRange(resultImportProduct);
							}
                        }
                        
                        var listImportProductDistinct = listImportProduct.Select(x=> new ImportProductDto
                        {
                            Quantity = 0,
                            Id = -1,
                            InboundReceiptId = x.InboundReceiptId,
                            ProductId = x.ProductId,
                            ProductName = x.ProductName,
                            SipplierName = x.SipplierName,
                            SupplierId = x.SupplierId,
                        }).DistinctBy(x=> new {x.SupplierId, x.ProductId }).ToList();
                        foreach(var item in listImportProductDistinct)
                        {
                            foreach(var item2 in listImportProduct)
                            {
                                if(item.ProductId == item2.ProductId && item.SupplierId == item2.SupplierId)
                                {
                                    item.Quantity += item2.Quantity;
                                }
                            }
                        }
                        worksheet.Cells[row + 3, 1].Value = listDate[j];
                        for(int k = 0;k< listImportProductDistinct.Count; k++)
                        {
                            worksheet.Cells[row + 3 + k, 2].Value = listImportProductDistinct[k].SipplierName;
                            worksheet.Cells[row + 3 + k, 3].Value = listImportProductDistinct[k].ProductName;
                            worksheet.Cells[row + 3 + k, 4].Value = listImportProductDistinct[k].Quantity;
                        }
                        row += listImportProductDistinct.Count;
					}
                    row += 3;

				}

				// Stream the Excel package to the client
				MemoryStream stream = new MemoryStream(package.GetAsByteArray());
				return File(stream, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "SelectedRows.xlsx");
			}
		}
	}
}
