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
    public class InboundReceiptController : Controller
    {
        private IInboundReceiptService _inboundReceiptService;
        public InboundReceiptController(IInboundReceiptService inboundReceiptService)
        {
            _inboundReceiptService = inboundReceiptService;
        }
        [HttpGet]
        public IActionResult GetAllInboundReceipt()
        {
            var result = _inboundReceiptService.GetAllInboundReceipt();
            return Ok(result);
        }
        [HttpGet]
        [Route("{Id}")]
        public IActionResult GetInboundReceipt(Guid Id)
        {
            var result = _inboundReceiptService.GetInboundReceipt(Id);
            return Ok(result);
        }
        [HttpPost]
        public IActionResult CreateInboundReceipt(InboundReceiptDto request)
        {
            var  result =_inboundReceiptService.CreateInboundReceipt(request);
            return Ok(result);
        }
        [HttpPut]
        [Route("{Id}")]
        public IActionResult EditInboundReceipt(InboundReceiptDto request)
        {
            var result =_inboundReceiptService.EditInboundReceipt(request);
            return Ok(result);
        }
        [HttpDelete]
        [Route("{Id}")]
        public IActionResult DeleteInboundReceipt(Guid Id)
        {
            var result = _inboundReceiptService.DeleteInboundReceipt(Id);
            return Ok(result);
        }
		[HttpPost]
		[Route("search")]
		public IActionResult Search(SearchRequest request)
		{
			var result = _inboundReceiptService.Search(request);
			return Ok(result);
		}
	}
}
