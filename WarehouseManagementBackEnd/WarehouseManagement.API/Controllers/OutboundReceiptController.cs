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
    public class OutboundReceiptController : Controller
    {
        private IOutboundReceiptService _outboundReceiptService;

        public OutboundReceiptController(IOutboundReceiptService outboundReceiptService)
        {
            _outboundReceiptService = outboundReceiptService;
        }

        [HttpGet]
        public IActionResult GetAllOutbound()
        {
            var result = _outboundReceiptService.GetAllOutboundReceipt();
            return Ok(result);
        }

        [HttpGet]
        [Route("{Id}")]
        public IActionResult GetOutboundReceipt(Guid Id)
        {
            var result = _outboundReceiptService.GetOutboundReceipt(Id);
            return Ok(result);
        }

        [HttpPost]
        public IActionResult CreateOutboundReceipt(OutboundReceiptDto request)
        {
            var result = _outboundReceiptService.CreateOutboundReceipt(request);    
            return Ok(result);
        }

        [HttpPut]
        [Route("{Id}")]
        public IActionResult EditOutboundReceipt(OutboundReceiptDto request)
        {
            var result = _outboundReceiptService.EditOutbountReceipt(request);
            return Ok(result);
        }

        [HttpDelete]
        [Route("Id")]
        public IActionResult DeleteOutboundReceipt(Guid Id)
        {
            var result = _outboundReceiptService.DeleteOutboundReceipt(Id);
            return Ok(result);
        }
		[HttpPost]
		[Route("search")]
		public IActionResult Search(SearchRequest request)
		{
			var result = _outboundReceiptService.Search(request);
			return Ok(result);
		}
	}
}
