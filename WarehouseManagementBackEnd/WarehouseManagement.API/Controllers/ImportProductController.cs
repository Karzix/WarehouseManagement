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
    //[Authorize(AuthenticationSchemes = "Bearer")]
    public class ImportProductController : Controller
    {
        private IImportProductService _importProductService;
        public ImportProductController(IImportProductService importProductService)
        {
            _importProductService = importProductService;
        }
        [HttpGet]
        public IActionResult GetAll()
        {
            var result =_importProductService.GetAllImportProduct();
            return Ok(result);
        }
        [HttpGet]
        [Route("{Id}")]
        public IActionResult Get(int Id)
        {
            var result =_importProductService.GetImportProduct(Id);
            return Ok(result);
        }
        [HttpPost]
        public IActionResult Create(ImportProductDto request)
        {
            var result = _importProductService.CreateImportProduct(request);
            return Ok(result);
        }
        [HttpDelete]
        [Route("{Id}")]
        public IActionResult Delete(int Id)
        {
            var result =_importProductService.DeleteImportProduct(Id);
            return Ok(result);
        }
        [HttpPut]
        [Route("{Id}")]
        public IActionResult Edit(ImportProductDto request)
        {
            var result =_importProductService.EditImportProduct(request);
            return Ok(result);
        }
		[HttpPost]
		[Route("search")]
		public IActionResult Search(SearchRequest request)
		{
			var result = _importProductService.Search(request);
			return Ok(result);
		}
	}
}
