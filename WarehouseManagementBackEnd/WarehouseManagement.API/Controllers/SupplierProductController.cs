using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WarehouseManagement.Model.Dto;
using WarehouseManagement.Service.Contract;

namespace WarehouseManagement.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SupplierProductController : ControllerBase
    {
        private ISupplierProductService _supplierProductService;

        public SupplierProductController(ISupplierProductService supplierProductService)
        {
            _supplierProductService = supplierProductService;
        }

        [HttpGet]
        public IActionResult GetAllSupplierProduct()
        {
            var result = _supplierProductService.GetAllSupplierProduct();
            return Ok(result);
        }
        [HttpGet]
        [Route("{Id}")]
        public IActionResult GetSupplierProduct(Guid Id)
        {
            var result = _supplierProductService.GetSupplierProduct(Id);
            return Ok(result);
        }
        [HttpPost]
        public IActionResult CreateSupplierProduct(SupplierProductDto request)
        {
            var result = _supplierProductService.CreateSupplierProduct(request);
            return Ok(result);
        }
        [HttpPut]
        [Route("Id")]
        public IActionResult EditSupplierProduct(SupplierProductDto request)
        {
            var result = _supplierProductService.EditSupplierProduct(request);
            return Ok(result);
        }
        [HttpDelete]
        [Route("{Id}")]
        public IActionResult DeleteSupplierProduct(Guid Id)
        {
            var result= _supplierProductService.DeleteSupplierProduct(Id);
            return Ok(result);
        }
    }
}
