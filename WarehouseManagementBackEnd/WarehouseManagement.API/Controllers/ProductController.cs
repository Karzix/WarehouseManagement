using Microsoft.AspNetCore.Mvc;
using WarehouseManagement.Model.Dto;
using WarehouseManagement.Service.Contract;

namespace WarehouseManagement.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    
    public class ProductController : ControllerBase
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
        public IActionResult GetProduct(Guid Id)
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
        public IActionResult DeleteProduct(Guid Id)
        {
            var result = _productService.DeleteProduct(Id);
            return Ok(result);
        }

        [HttpPut]
        [Route("{Id}")]
        public IActionResult EditProduct(ProductDto request)
        {
            var result= _productService.EditProduct(request);
            return Ok(result);
        }

    }
}
