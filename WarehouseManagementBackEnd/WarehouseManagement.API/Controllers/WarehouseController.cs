using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WarehouseManagement.Model.Dto;
using WarehouseManagement.Service.Contract;

namespace WarehouseManagement.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
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
        public IActionResult GetWarehouse(Guid id)
        {
            var result = _warehouseService.GetWarehouseById(id);
            return Ok(result);
        }
        [HttpPost]
        public IActionResult CreateWarehouse(WarehouseDto request)
        {
            var result = _warehouseService.CreateWarehouse(request);
            return Ok(result);
        }
        [HttpPut]
        [Route("{id}")]
        public IActionResult EditWarehouse(WarehouseDto request)
        {
            var result = _warehouseService.EditWarehouse(request);
            return Ok(result);
        }
        [HttpDelete]
        public IActionResult DeleteWarehouse(Guid id)
        {

            var result = _warehouseService.DeleteWarehouse(id);

            return Ok(result);

        }
    }
}
