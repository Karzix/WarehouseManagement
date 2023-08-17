using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MayNghien.Models.Response.Base;
using Microsoft.AspNetCore.Identity;
using WarehouseManagement.Model.Dto;

namespace WarehouseManagement.Service.Contract
{
    public interface IUserManagementService
    {
        AppResponse<List<UserModel>> GetAllUser();
        Task<AppResponse<string>> ResetPassword(Guid Id);
        Task<AppResponse<string>> CreateUser(UserModel user);

    }
}
