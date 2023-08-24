using MayNghien.Models.Request.Base;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WarehouseManagement.Model.Dto;
using WarehouseManagement.Service.Contract;

namespace WarehouseManagement.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserManagementController : Controller
    {
        IUserManagementService _userManagementService;
        public UserManagementController(IUserManagementService userManagementService)
        {
            _userManagementService = userManagementService;
        }
        [HttpGet]
        public IActionResult GetAllUser()
        {
            var result = _userManagementService.GetAllUser();
            return Ok(result);
        }
        [HttpPut]
        [Route("{Id}")]
        public async Task<IActionResult> ResetPassword(Guid Id)
        {
            var result = await _userManagementService.ResetPassword(Id);
            return Ok(result);
        }
        [HttpPost]
        public async Task<IActionResult> CreateUser(UserModel userModel)
        {
            var result = await _userManagementService.CreateUser(userModel);
            return Ok(result);
        }
        [HttpGet]
        [Route("search")]
        public async Task<IActionResult> Search(SearchRequest request)
        {
            var result = await _userManagementService.Search(request);
            return Ok(result);
        }
    }
}
