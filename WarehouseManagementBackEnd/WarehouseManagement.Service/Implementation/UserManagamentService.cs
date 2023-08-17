using AutoMapper;
using MayNghien.Models.Response.Base;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using WarehouseManagement.Common.Enum;
using WarehouseManagement.DAL.Models.Context;
using WarehouseManagement.Model.Dto;
using WarehouseManagement.Service.Contract;
using static MayNghien.Common.CommonMessage.AuthResponseMessage;

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

        public async Task<AppResponse<string>> CreateUser(UserModel user)
        {
            var result = new AppResponse<string>();
            try
            {
                if (string.IsNullOrEmpty(user.Email))
                {
                    return result.BuildError(ERR_MSG_EmailIsNullOrEmpty);
                }
                var identityUser = await _userManager.FindByNameAsync(user.UserName);
                if (identityUser != null)
                {
                    return result.BuildError(ERR_MSG_UserExisted);
                }
                var newIdentityUser = new IdentityUser { Email = user.Email, UserName = user.Email };
                var createResult = await _userManager.CreateAsync(newIdentityUser);
                await _userManager.AddPasswordAsync(newIdentityUser, user.Password);

                //newIdentityUser = await _userManager.FindByEmailAsync(user.Email);
                //if (newIdentityUser != null)
                //{
                //    if (user.Role != null && user.Role == nameof(UserRoleEnum.TenantAdmin))
                //    {
                //        var AccountInfo = new AccountInfo()
                //        {
                //            Id = Guid.NewGuid(),
                //            Balance = 0,
                //            Email = user.Email,
                //            CreatedBy = user.Email,
                //            CreatedOn = DateTime.Now,
                //            Name = user.UserName,
                //            IsDeleted = false,
                //            UserId = newIdentityUser.Id,
                //        };
                //        _accountInfoRepository.Add(AccountInfo);
                //    }
                //    await _userManager.AddToRoleAsync(newIdentityUser, user.Role);
                //}
                return result.BuildResult(INFO_MSG_UserCreated);
            }
            catch (Exception ex)
            {

                return result.BuildError(ex.ToString());
            }
        }
        public async Task<AppResponse<string>> DeleteUser(string id)
        {
            var result = new AppResponse<string>();
            try
            {

                IdentityUser identityUser = new IdentityUser();

                identityUser = await _userManager.FindByIdAsync(id);
                if (identityUser != null)
                {
                    if (await _userManager.IsInRoleAsync(identityUser, "tenant"))
                    {
                        //var qUserInfo = _accountInfoRepository.FindBy(m => m.UserId == id && m.IsDeleted == false);
                        //if (qUserInfo.Count() > 0)
                        //{
                        //    var AccountInfo = qUserInfo.FirstOrDefault();
                        //    AccountInfo.IsDeleted = true;
                        //    _accountInfoRepository.Edit(AccountInfo);
                        //}
                        //await _userManager.SetLockoutEnabledAsync(identityUser, true);
                        var user = _context.Users.FirstOrDefault(m => m.Id == id);
                        _context.Users.Remove(user);
                    }

                }
                return result.BuildResult(/*INFO_MSG_UserDeleted*/"Đã xóa");
            }
            catch (Exception ex)
            {

                return result.BuildError(ex.ToString());
            }
        }
    }
}
