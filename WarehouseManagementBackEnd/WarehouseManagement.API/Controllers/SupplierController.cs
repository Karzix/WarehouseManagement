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
    public class SupplierController : Controller
    {
        private ISupplierService _supplierService;

        public SupplierController(ISupplierService supplierService)
        {
            _supplierService = supplierService;
        }

        [HttpGet]
        public IActionResult GetAllSupplier()
        {
            var result = _supplierService.GetAllSupplier();
            return Ok(result);
        }

        [HttpGet]
        [Route("{Id}")]
        public IActionResult GetSupplier(int Id)
        {
            var result = _supplierService.GetSupplier(Id);
            return Ok(result);
        }

        [HttpDelete]
        [Route("{Id}")]
        public IActionResult DeleteSupplier(int Id)
        {
            var result = _supplierService.DeleteSupplier(Id);
            return Ok(result);
        }

        [HttpPost]
        public IActionResult CreateSupplier(SupplierDto supplier)
        {
            var result = _supplierService.CreateSupplier(supplier);
            return Ok(result);
        }

        [HttpPut]
        //[Route("{Id}")]
        public IActionResult EditSupplier(SupplierDto supplier)
        {
            var result = _supplierService.EditSupplier(supplier);
            return Ok(result);
        }
		[HttpPost]
		[Route("search")]
		public IActionResult Search(SearchRequest request)
		{
			var result = _supplierService.Search(request);
			return Ok(result);
		}
	}
}
