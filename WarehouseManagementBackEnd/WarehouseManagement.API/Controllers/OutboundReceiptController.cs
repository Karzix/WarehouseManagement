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
			var listOutboundReceipt = new List<OutboundReceiptDto>();
			listOutboundReceipt = _outboundReceiptService.Search(request).Data.Data;

			using (var package = new ExcelPackage())
			{
				var worksheet = package.Workbook.Worksheets.Add("SelectedRows");
				int row = 0;
				for (int i = 0; i < listOutboundReceipt.Count; i++)
				{
					worksheet.Cells[row + 1, 1].Value = "Kho: " + listOutboundReceipt[i].WarehouseName;
					worksheet.Cells[row + 2, 1].Value = "Nơi đến: " + listOutboundReceipt[i].To;
					worksheet.Cells[row + 2, 3].Value = "Thời gian nhập: " + listOutboundReceipt[i].CreatedOn.ToString();
					var searchExportProduct = new SearchRequest
					{
						PageIndex = 1,
						PageSize = int.MaxValue
					};

					searchExportProduct.Filters = new List<Filter>();
					searchExportProduct.Filters.Add(new Filter
					{
						FieldName = "IsDelete",
						Value = "False"
					});

					searchExportProduct.Filters.Add(new Filter
					{
						FieldName = "OutboundReceiptId",
						Value = listOutboundReceipt[i].Id.ToString()
					});

					var listExportProduct = new List<ExportProductDto>();
					listExportProduct  =  _exportProductService.Search(searchExportProduct).Data.Data;

					worksheet.Cells[row + 3, 1].Value = "Sản phẩm";
					worksheet.Cells[row + 3, 2].Value = "Nhà cung cấp";
					worksheet.Cells[row + 3, 3].Value = "Số lượng";

					row += 4;

					for (int j = 0; j < listExportProduct.Count; j++)
					{
						worksheet.Cells[row + j, 1].Value = listExportProduct[j].ProductName;
						worksheet.Cells[row + j, 2].Value = listExportProduct[j].SupplierName;
						worksheet.Cells[row + j, 3].Value = listExportProduct[j].Quantity.ToString();
					}

					row += listExportProduct.Count;
				}

				// Stream the Excel package to the client
				MemoryStream stream = new MemoryStream(package.GetAsByteArray());
				return File(stream, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "SelectedRows.xlsx");
			}
		}
	}
}
