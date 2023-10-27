using MayNghien.Models.Request.Base;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using OfficeOpenXml;
using WarehouseManagement.Model.Dto;
using WarehouseManagement.Service.Contract;
using WarehouseManagement.Service.Implementation;

namespace WarehouseManagement.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
	//[Authorize(AuthenticationSchemes = "Bearer")]
	public class OutboundReceiptController : Controller
    {
        private IOutboundReceiptService _outboundReceiptService;
		private IExportProductService _exportProductService;

        public OutboundReceiptController(IOutboundReceiptService outboundReceiptService, IExportProductService exportProductService)
        {
            _outboundReceiptService = outboundReceiptService;
			_exportProductService = exportProductService;
        }

        [HttpGet]
        public IActionResult GetAllOutbound()
        {
            var result = _outboundReceiptService.GetAllOutboundReceipt();
            return Ok(result);
        }

        [HttpGet]
        [Route("{Id}")]
        public IActionResult GetOutboundReceipt(int Id)
        {
            var result = _outboundReceiptService.GetOutboundReceipt(Id);
            return Ok(result);
        }

        [HttpPost]
        public IActionResult CreateOutboundReceipt(OutboundReceiptDto request)
        {
            var result = _outboundReceiptService.CreateOutboundReceipt(request);    
            return Ok(result);
        }

        [HttpPut]
        [Route("{Id}")]
        public IActionResult EditOutboundReceipt(OutboundReceiptDto request)
        {
            var result = _outboundReceiptService.EditOutbountReceipt(request);
            return Ok(result);
        }

        [HttpDelete]
        [Route("Id")]
        public IActionResult DeleteOutboundReceipt(int Id)
        {
            var result = _outboundReceiptService.DeleteOutboundReceipt(Id);
            return Ok(result);
        }
		[HttpPost]
		[Route("search")]
		public IActionResult Search(SearchRequest request)
		{
			var result = _outboundReceiptService.Search(request);
			return Ok(result);
		}
        [HttpPost("Download")]
        public IActionResult DownloadSelectedRows(SearchRequest request)
        {
			request.PageSize = int.MaxValue;
			request.PageIndex = 1;
			var listInboundReceipt = new List<OutboundReceiptDto>();
			listInboundReceipt = _outboundReceiptService.Search(request).Data.Data;
			using (var package = new ExcelPackage())
			{
				var worksheet = package.Workbook.Worksheets.Add("SelectedRows");
				int row = 0;
				int countInboundReceipt = 0;
				var listWarehouseId = listInboundReceipt.Select(x => x.WarehouseId).Distinct().ToList();
				for (int i = 0; i < listWarehouseId.Count; i++)
				{
					var listOutboundReceiptForWarehouseID = listInboundReceipt.Where(x => x.WarehouseId == listWarehouseId[i]).OrderBy(x => x.CreatedOn).ToList();
					worksheet.Cells[row + 1, 1].Value = "Kho: " + listOutboundReceiptForWarehouseID[0].WarehouseName;
					worksheet.Cells[row + 2, 1].Value = "Ngày nhập";
					worksheet.Cells[row + 2, 2].Value = "Nhà cung cấp";
					worksheet.Cells[row + 2, 3].Value = "Sản phẩm";
					worksheet.Cells[row + 2, 4].Value = "Số lượng";
					var listDate = listOutboundReceiptForWarehouseID.Select(x => x.CreatedOn.Value.ToString("dd/MM/yyyy")).Distinct().ToList();
					for (int j = 0; j < listDate.Count; j++)
					{

						var listExportProduct = new List<ExportProductDto>();
						for (int z = 0; z < listOutboundReceiptForWarehouseID.Count; z++)
						{
							if (listOutboundReceiptForWarehouseID[z].CreatedOn.Value.ToString("dd/MM/yyyy") == listDate[j])
							{
								var searchExportProduct = new SearchRequest();
								searchExportProduct.Filters = new List<Filter>();
								searchExportProduct.PageSize = int.MaxValue;
								searchExportProduct.PageIndex = 1;
								searchExportProduct.Filters.Add(new Filter
								{
									FieldName = "Day",
									Value = listDate[j],
									Operation = ""
								});
								searchExportProduct.Filters.Add(new Filter
								{
									FieldName = "IsDelete",
									Value = "false",
									Operation = ""
								});
								searchExportProduct.Filters.Add(new Filter
								{
									FieldName = "OutboundReceiptId",
									Value = listOutboundReceiptForWarehouseID[z].Id.ToString(),
									Operation = ""
								});
								var resultExportProduct = _exportProductService.Search(searchExportProduct).Data.Data;
								listExportProduct.AddRange(resultExportProduct);
							}
						}

						var listExportProductDistinct = listExportProduct.Select(x => new ExportProductDto
						{
							Quantity = 0,
							Id = -1,
							OutboundReceiptId = x.OutboundReceiptId,
							ProductName = x.ProductName,
							SupplierName = x.SupplierName,
							SupplierId = x.SupplierId,
							ProductId = x.ProductId,
						}).DistinctBy(x => new { x.SupplierId, x.ProductId }).ToList();
						foreach (var item in listExportProductDistinct)
						{
							foreach (var item2 in listExportProduct)
							{
								if (item.ProductId == item2.ProductId && item.SupplierId == item2.SupplierId)
								{
									item.Quantity += item2.Quantity;
								}
							}
						}
						worksheet.Cells[row + 3, 1].Value = listDate[j];
						for (int k = 0; k < listExportProductDistinct.Count; k++)
						{
							worksheet.Cells[row + 3 + k, 2].Value = listExportProductDistinct[k].SupplierName;
							worksheet.Cells[row + 3 + k, 3].Value = listExportProductDistinct[k].ProductName;
							worksheet.Cells[row + 3 + k, 4].Value = listExportProductDistinct[k].Quantity;
						}
						row += listExportProductDistinct.Count;
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
