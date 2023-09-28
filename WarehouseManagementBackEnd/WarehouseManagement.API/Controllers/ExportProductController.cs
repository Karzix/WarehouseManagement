using MayNghien.Models.Request.Base;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WarehouseManagement.Model.Dto;
using WarehouseManagement.Service.Contract;
using WarehouseManagement.Service.Implementation;

namespace WarehouseManagement.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(AuthenticationSchemes = "Bearer")]
    public class ExportProductController : Controller
    {
        private IExportProductService _exportProductService;

        public ExportProductController(IExportProductService exportProductService)
        {
            _exportProductService = exportProductService;
        }
        [HttpGet]
        public IActionResult GetAllExportProduct()
        {
            var result = _exportProductService.GetAllExportProduct();
            return Ok(result);
        }
        [HttpGet]
        [Route("{Id}")]
        public IActionResult GetExportProduct(Guid Id)
        {
            var result = _exportProductService.GetExportProduct(Id);
            return Ok(result);
        }
        [HttpPost]
        public IActionResult CreateExportProduct(ExportProductDto request)
        {
            var result = _exportProductService.CreateExportProduct(request);
            return Ok(result);
        }
        [HttpPut]
        [Route("{Id}")]
        public IActionResult EditExportProduct(ExportProductDto request)
        {
            var result = _exportProductService.EditExportProduct(request);
            return Ok(result);
        }
        [HttpDelete]
        [Route("{Id}")]
        public IActionResult DeleteExportProduct(Guid Id)
        {
            var result =_exportProductService.DeleteExportProduct(Id);
            return Ok(result);
        }
		[HttpPost]
		[Route("search")]
		public IActionResult Search(SearchRequest request)
		{
			var result = _exportProductService.Search(request);
			return Ok(result);
		}
	}
}
