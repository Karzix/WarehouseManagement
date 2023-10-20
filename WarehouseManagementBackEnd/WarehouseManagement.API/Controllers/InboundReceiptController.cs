using System.Data.Entity.Core.Common.CommandTrees.ExpressionBuilder;
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
			var listInboundReceipt = _inboundReceiptService.Search(request).Data.Data;

			using (var package = new ExcelPackage())
			{
				var worksheet = package.Workbook.Worksheets.Add("SelectedRows");
				int row = 0;
				for (int i = 0; i < listInboundReceipt.Count; i++)
				{
					worksheet.Cells[row + 1, 1].Value = "Kho: " + listInboundReceipt[i].WarehouseName;
					worksheet.Cells[row + 2, 1].Value = "Nhà cung cấp: " + listInboundReceipt[i].SupplierName;
					worksheet.Cells[row +2,3].Value="Thời gian nhập: " + listInboundReceipt[i].CreatedOn.ToString();
					var searchImportProduct = new SearchRequest
					{
						PageIndex = 1,
						PageSize = int.MaxValue
					};

					searchImportProduct.Filters = new List<Filter>();
					searchImportProduct.Filters.Add(new Filter
					{
						FieldName = "IsDelete",
						Value = "False"
					});

					searchImportProduct.Filters.Add(new Filter
					{
						FieldName = "InboundReceiptId",
						Value = listInboundReceipt[i].Id.ToString()
					});

					var listImportProduct = _importProductService.Search(searchImportProduct).Data.Data;

					worksheet.Cells[row + 3, 1].Value = "Sản phẩm";
					worksheet.Cells[row + 3, 2].Value = "Số lượng";

					row += 4;

					for (int j = 0; j < listImportProduct.Count; j++)
					{
						worksheet.Cells[row + j, 1].Value = listImportProduct[j].ProductName;
						worksheet.Cells[row + j, 2].Value = listImportProduct[j].Quantity.ToString();
					}

					row += listImportProduct.Count;
				}

				// Stream the Excel package to the client
				MemoryStream stream = new MemoryStream(package.GetAsByteArray());
				return File(stream, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "SelectedRows.xlsx");
			}
		}
	}
}
