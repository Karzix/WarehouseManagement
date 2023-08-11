using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MayNghien.Models.Response.Base;
using WarehouseManagement.Model.Dto;

namespace WarehouseManagement.Service.Contract
{
    public interface IAuthService
    {
        Task<AppResponse<string>> AuthenticateUser(UserModel user);
        Task<AppResponse<string>> CreateUser(UserModel user);
    }
}
