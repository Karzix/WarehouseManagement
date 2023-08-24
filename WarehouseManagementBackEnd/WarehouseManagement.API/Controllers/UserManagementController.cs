using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
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
    }
}
