using MayNghien.Models.Request.Base;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
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
            var result= _productService.EditProduct(request);
            return Ok(result);
        }
        [HttpPost]
        [Route("search")]
        public IActionResult Search(SearchRequest request)
        {
            var result =_productService.Search(request);
            return Ok(result);
        }
    }
}
