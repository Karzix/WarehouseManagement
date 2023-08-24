﻿using AutoMapper;
using MayNghien.Models.Request.Base;
using MayNghien.Models.Response.Base;
using Microsoft.AspNetCore.Identity;
using WarehouseManagement.DAL.Contract;
using WarehouseManagement.Model.Dto;
using WarehouseManagement.Model.Response.User;
using WarehouseManagement.Service.Contract;
using LinqKit;
using static Maynghien.Common.Helpers.SearchHelper;
using static MayNghien.Common.CommonMessage.AuthResponseMessage;

namespace WarehouseManagement.Service.Implementation
{
    public class UserManagamentService : IUserManagementService
    {
        //private WarehouseManagementDbContext _context;
        private readonly IMapper _mapper;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly IUserManagementRepository _userManagementRepository;

        public UserManagamentService(IMapper mapper,UserManager<IdentityUser> userManager, IUserManagementRepository userManagementRepository)
        {
            
            _mapper = mapper;
            _userManager = userManager;
            _userManagementRepository = userManagementRepository;
        }


        public AppResponse<List<IdentityUser>> GetAllUser()
        {
            
            var result = new AppResponse<List<IdentityUser>>();
            try
            {
                var list = _userManagementRepository.GetAll();

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
                var user = _userManagementRepository.GetById(Id.ToString());
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
                        var user = _userManagementRepository.GetById(id);
                        await _userManager.DeleteAsync(user);
                    }

                }
                return result.BuildResult(/*INFO_MSG_UserDeleted*/"Đã xóa");
            }
            catch (Exception ex)
            {

                return result.BuildError(ex.ToString());
            }
        }

        public async Task<AppResponse<SearchUserResponse>> Search(SearchRequest request)
        {
            var result = new AppResponse<SearchUserResponse>();
            try
            {
                var query = BuildFilterExpression(request.Filters);
                var numOfRecords = _userManagementRepository.CountRecordsByPredicate(query);

                var users = _userManagementRepository.FindByPredicate(query);
                int pageIndex = request.PageSize ?? 1;
                int pageSize = request.PageSize ?? 1;
                int startIndex = (pageIndex - 1) * (int)pageSize;
                var UserList = users.Skip(startIndex).Take(pageSize).ToList();
                var dtoList = _mapper.Map<List<UserModel>>(UserList);
                if (dtoList != null && dtoList.Count > 0)
                {
                    for (int i = 0; i < UserList.Count; i++)
                    {
                        var dtouser = dtoList[i];
                        var identityUser = UserList[i];
                        dtouser.Role = (await _userManager.GetRolesAsync(identityUser)).First();
                    }
                }
                var searchUserResult = new SearchUserResponse
                {
                    TotalRows = numOfRecords,
                    TotalPages = CalculateNumOfPages(numOfRecords, pageSize),
                    CurrentPage = pageIndex,
                    Data = dtoList,
                };

                return result.BuildResult(searchUserResult);

            }
            catch (Exception ex)
            {

                return result.BuildError(ex.ToString());
            }
        }

        //public async Task<AppResponse<string>> EditUser(UserModel user)
        //{
        //    var result = new AppResponse<string>();
        //    try
        //    {
        //        var identityUser = await _userManager.FindByIdAsync(user.Id);
        //        if (identityUser != null)
        //        {
        //            identityUser = _mapper.Map<IdentityUser>(user);
        //            _userManagementRepository.EditUser(identityUser);
        //            return result.BuildResult("ok");
        //        }
        //        return result.BuildResult("Không tim thấy người dùng");
        //    }
        //    catch(Exception ex)
        //    {
        //        result.IsSuccess = false;
        //        result.Message = ex.Message + " " + ex.StackTrace;
        //        return result;
        //    }
        //}

        private ExpressionStarter<IdentityUser> BuildFilterExpression(IList<Filter> Filters)
        {
            try
            {
                var predicate = PredicateBuilder.New<IdentityUser>(true);

                foreach (var filter in Filters)
                {
                    switch (filter.FieldName)
                    {
                        case "Email":
                            predicate = predicate.And(m => m.Email.Equals(filter.Value));
                            break;

                        default:
                            break;
                    }
                }
                return predicate;
            }
            catch (Exception)
            {

                throw;
            }
        }
    }
}