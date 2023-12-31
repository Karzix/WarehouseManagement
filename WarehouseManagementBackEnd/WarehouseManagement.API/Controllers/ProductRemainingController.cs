﻿using MayNghien.Models.Request.Base;
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
    public class ProductRemainingController : Controller
    {
        private IProductRemainingService _productRemainingService;

        public ProductRemainingController(IProductRemainingService productRemainingService)
        {
            _productRemainingService = productRemainingService;
        }

        [HttpGet]
        public IActionResult GetAllProductRemaining()
        {
            var result = _productRemainingService.GetAllProductRemaining();
            return Ok(result);
        }
        [HttpGet]
        [Route("{Id}")]
        public IActionResult GetProductRemaining(int Id)
        {
            var result= _productRemainingService.GetProductRemaining(Id);
            return Ok(result);
        }
        [HttpPost]
        public IActionResult CreateProductRemaining(ProductRemainingDto request)
        {
            var result = _productRemainingService.CreateProductRemaining(request);
            return Ok(result);
        }
        [HttpDelete]
        [Route("{Id}")]
        public IActionResult DeleteProductRemaining(int Id)
        {
            var result = _productRemainingService.DeleteProductRemaining(Id);
            return Ok(result);
        }
        [HttpPut]
        [Route("{Id}")]
        public IActionResult EditProductRemaining(ProductRemainingDto request)
        {
            var result = _productRemainingService.EditProductRemaining(request);
            return Ok(result);
        }
		[HttpPost]
		[Route("search")]
		public IActionResult Search(SearchRequest request)
		{
			var result = _productRemainingService.Search(request);
			return Ok(result);
		}
	}
}
