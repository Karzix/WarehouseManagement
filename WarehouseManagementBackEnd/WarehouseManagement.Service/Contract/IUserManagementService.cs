using MayNghien.Models.Request.Base;
using MayNghien.Models.Response.Base;
using Microsoft.AspNetCore.Identity;
using WarehouseManagement.Model.Dto;
using WarehouseManagement.Model.Response.User;

namespace WarehouseManagement.Service.Contract
{
    public interface IUserManagementService
    {
        AppResponse<List<IdentityUser>> GetAllUser();
        Task<AppResponse<string>> ResetPassword(Guid Id);
        Task<AppResponse<string>> CreateUser(UserModel user);
        //AppResponse<SearchRequest> Search(SearchRequest request);
        //Task<AppResponse<string>> EditUser(UserModel user);
        Task<AppResponse<SearchUserResponse>> Search(SearchRequest request);
        Task<AppResponse<string>> DeleteUser(string id);
        Task<AppResponse<UserModel>> GetUserAsync(string id);
    }
}
