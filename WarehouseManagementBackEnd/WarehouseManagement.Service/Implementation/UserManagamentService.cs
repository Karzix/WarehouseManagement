using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using AutoMapper;
using MayNghien.Models.Response.Base;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using RTools_NTS.Util;
using WarehouseManagement.DAL.Models.Context;
using WarehouseManagement.Model.Dto;
using WarehouseManagement.Service.Contract;

namespace WarehouseManagement.Service.Implementation
{
    public class UserManagamentService : IUserManagementService
    {
        private WarehouseManagementDbContext _context;
        private readonly IMapper _mapper;
        private readonly UserManager<IdentityUser> _userManager;

        public UserManagamentService(WarehouseManagementDbContext context, IMapper mapper, UserManager<IdentityUser> userManager)
        {
            _context = context;
            _mapper = mapper;
            _userManager = userManager;
        }


        public AppResponse<List<UserModel>> GetAllUser()
        {
            
            var result = new AppResponse<List<UserModel>>();
            try
            {
                var list = (from user in _context.Users
                            join userRole in _context.UserRoles on user.Id equals userRole.UserId
                            join role in _context.Roles on userRole.RoleId equals role.Id
                           select new UserModel
                           {
                               Id = Guid.Parse(user.Id),
                               UserName = user.UserName,
                               Password = user.PasswordHash,
                               Role = role.Name,
                               Email = user.Email
                           }).ToList();


                result.IsSuccess = true;
                result.Data = list;
                return result;
            }
            catch (Exception ex)
            {
                result.IsSuccess = false;
                result.Message = ex.Message + " " + ex.StackTrace;
                return result;
            }
        }

        public async Task<AppResponse<string>> ResetPassword(Guid Id)
        {
            var result = new AppResponse<string>();
            try
            {
                //dfd1b9a6-ce48-43c6-8ca2-1776b82a4fc8
                var user = _context.Users.FirstOrDefault(m=> m.Id == Id.ToString());
                await _userManager.RemovePasswordAsync(user);
                await _userManager.AddPasswordAsync(user, "CoCaiMatKhauCungQuen");
                result.IsSuccess = true;
                result.Data = "CoCaiMatKhauCungQuen";
                return result;
            }
            catch(Exception ex)
            {
                result.IsSuccess = false;
                result.Message = ex.Message + " " + ex.StackTrace;
                return result;
            }
            

        }
    }
}
