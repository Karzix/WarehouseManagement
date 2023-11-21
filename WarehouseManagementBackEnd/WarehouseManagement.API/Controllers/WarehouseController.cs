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
    public class WarehouseController : Controller
    {
        private readonly IWarehouseService _warehouseService;
        public WarehouseController(IWarehouseService warehouseService)
        {
            _warehouseService = warehouseService;
        }
        [HttpGet]
        public IActionResult GetAll()
        {
            var result = _warehouseService.GetAllWarehouse();
            return Ok(result);
        }
        [HttpGet]
        [Route("{Id}")]
        public IActionResult GetWarehouse(int Id)
        {
            var result = _warehouseService.GetWarehouseById(Id);
            return Ok(result);
        }
        [HttpPost]
        public IActionResult CreateWarehouse(WarehouseDto request)
        {
            var result = _warehouseService.CreateWarehouse(request);
            return Ok(result);
        }
        [HttpPut]
        //[Route("{id}")]
        public IActionResult EditWarehouse(WarehouseDto request)
        {
            var result = _warehouseService.EditWarehouse(request);
            return Ok(result);
        }
        [HttpDelete]
        [Route("{id}")]
        public IActionResult DeleteWarehouse(int id)
        {

            var result = _warehouseService.DeleteWarehouse(id);

            return Ok(result);

        }
		[HttpPost]
		[Route("search")]
		public IActionResult FindProduct(SearchRequest search)
		{
			var result = _warehouseService.Search(search);
			return Ok(result);
		}
	}
}
