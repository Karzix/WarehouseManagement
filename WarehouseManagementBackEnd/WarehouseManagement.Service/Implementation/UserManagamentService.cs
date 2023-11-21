using AutoMapper;
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
using WarehouseManagement.DAL.Implementation;
using WarehouseManagement.DAL.Models.Entity;

namespace WarehouseManagement.Service.Implementation
{
    public class UserManagamentService : IUserManagementService
    {
        //private WarehouseManagementDbContext _context;
        private readonly IMapper _mapper;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly IUserManagementRepository _userManagementRepository;
		private readonly RoleManager<IdentityRole> _roleManager;

		public UserManagamentService(IMapper mapper,UserManager<IdentityUser> userManager, IUserManagementRepository userManagementRepository, RoleManager<IdentityRole> roleManager)
        {
            
            _mapper = mapper;
            _userManager = userManager;
            _userManagementRepository = userManagementRepository;
            _roleManager = roleManager;
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
                var user = _userManagementRepository.FindById(Id.ToString());
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
                var identityUser = await _userManager.FindByNameAsync(user.Email);
                if (identityUser != null)
                {
                    return result.BuildError(ERR_MSG_UserExisted);
                }
                var newIdentityUser = new IdentityUser { Email = user.Email, UserName = user.UserName };
                var createResult = await _userManager.CreateAsync(newIdentityUser);
                await _userManager.AddPasswordAsync(newIdentityUser, user.Password);
				if (!(await _roleManager.RoleExistsAsync(user.Role)))
				{
					IdentityRole role = new IdentityRole { Name = user.Role };
					await _roleManager.CreateAsync(role);
				}
				await _userManager.AddToRoleAsync(newIdentityUser, user.Role);
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
                    //if (await _userManager.IsInRoleAsync(identityUser, "tenant"))
                    //{
                        await _userManager.DeleteAsync(identityUser);
                    //}

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
                int pageIndex = request.PageIndex ?? 1;
                int pageSize = request.PageSize ?? 1;
                int startIndex = (pageIndex - 1) * (int)pageSize;
                var UserList = users.Skip(startIndex).Take(pageSize).ToList();
                var dtoList = UserList.Select(x => new UserModel
                {
                    Email = x.Email,
                    UserName = x.UserName,
                    Id = x.Id
                }).ToList();
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

                result.Data = searchUserResult;
                result.IsSuccess = true;

                return result;

            }
            catch (Exception ex)
            {

                return result.BuildError(ex.ToString());
            }
        }


        private ExpressionStarter<IdentityUser> BuildFilterExpression(List<Filter> Filters)
        {
            try
            {
                var predicate = PredicateBuilder.New<IdentityUser>(true);
                if (Filters!=null)
                foreach (var filter in Filters)
                {
                    switch (filter.FieldName)
                    {
                        case "userName":
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

        public async Task<AppResponse<UserModel>> GetUserAsync(string id)
        {
            var result = new AppResponse<UserModel>();
            try
            {
                List<Filter> Filters = new List<Filter>();
                var query = BuildFilterExpression(Filters);

                var identityUser = _userManagementRepository.FindById(id);

                if (identityUser == null)
                {
                    return result.BuildError("User not found");
                }
                var dtouser = _mapper.Map<UserModel>(identityUser);

                dtouser.Role = (await _userManager.GetRolesAsync(identityUser)).First();

                result.Data = dtouser;
                result.IsSuccess = true;

                return result;
            }
            catch (Exception ex)
            {
                result.IsSuccess = false;
                result.Message = ex.Message + " " + ex.StackTrace;
                return result.BuildError(ex.ToString());
            }
        }
    }
}
