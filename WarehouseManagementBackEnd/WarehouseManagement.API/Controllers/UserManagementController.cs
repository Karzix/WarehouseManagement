using MayNghien.Models.Request.Base;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WarehouseManagement.Model.Dto;
using WarehouseManagement.Service.Contract;

namespace WarehouseManagement.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    //[Authorize(AuthenticationSchemes = "Bearer")]
    public class UserManagementController : Controller
    {
        IUserManagementService _userManagementService;
        public UserManagementController(IUserManagementService userManagementService)
        {
            _userManagementService = userManagementService;
        }
        [HttpGet]
        //[AllowAnonymous]
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
        [HttpPost]
        [Route("search")]
        public async Task<IActionResult> Search([FromBody] SearchRequest search)
        {
            var result = await _userManagementService.Search(search);
            return Ok(result);
        }
        [HttpGet]
        [Route("{Id}")]
        public async Task<IActionResult> GetUser(string Id)
        {
            var result = await _userManagementService.GetUserAsync(Id);
            return Ok(result);
        }
        [HttpDelete]
        [Route("{Id}")]
        public async Task<IActionResult> DeleteUser(string Id)
        {
            var result = await _userManagementService.DeleteUser(Id);
            return Ok(result);
        }
    }
}
