using MayNghien.Models.Request.Base;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OfficeOpenXml;
using WarehouseManagement.Model.Dto;
using WarehouseManagement.Service.Contract;

namespace WarehouseManagement.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(AuthenticationSchemes = "Bearer")]
    public class ProductController : Controller
    {
        private IProductService _productService;

        public ProductController(IProductService productService)
        {
            _productService = productService;
        }
        [HttpGet]
        public IActionResult GetAllProduct()
        {
            var result = _productService.GetAllProduct();
            return Ok(result);
        }
        [HttpGet]
        [Route("{Id}")]
        public IActionResult GetProduct(int Id)
        {
            var result = _productService.GetProduct(Id);
            return Ok(result);
        }
        [HttpPost]
        public IActionResult CreateProduct(ProductDto request)
        {
            var result = _productService.CreateProduct(request);
            return Ok(result);
        }
        [HttpDelete]
        [Route("{Id}")]
        public IActionResult DeleteProduct(int Id)
        {
            var result = _productService.DeleteProduct(Id);
            return Ok(result);
        }

        [HttpPut]
        [Route("{Id}")]
        public IActionResult EditProduct(ProductDto request)
        {
            var result = _productService.EditProduct(request);
            return Ok(result);
        }
        [HttpPost]
        [Route("search")]
        public IActionResult Search(SearchRequest request)
        {
            var result = _productService.Search(request);
            return Ok(result);
        }

		[HttpPost("Download")]
		public IActionResult DownloadSelectedRows(SearchRequest request)
		{
            var data = _productService.Search(request);

			using (var package = new ExcelPackage())
			{
				var worksheet = package.Workbook.Worksheets.Add("SelectedRows");
                worksheet.Cells[1, 1].Value = "Name";
                worksheet.Cells[1,2].Value = "Quantity";
				// Populate the worksheet with your data
				for (var i = 0; i < data.Data.Data.Count; i++)
				{
					// Modify this section based on your entity properties
					worksheet.Cells[i + 1 +1, 1].Value = data.Data.Data[i].Name;
					worksheet.Cells[i + 1 +1, 2].Value = data.Data.Data[i].Quantity;
					// Add more properties as needed
				}

				// Stream the Excel package to the client
				MemoryStream stream = new MemoryStream(package.GetAsByteArray());
				return File(stream, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "SelectedRows.xlsx");
			}
		}
	}
}
