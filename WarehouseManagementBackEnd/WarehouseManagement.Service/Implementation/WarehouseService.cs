using AutoMapper;
using LinqKit;
using MayNghien.Common.Helpers;
using MayNghien.Models.Request.Base;
using MayNghien.Models.Response.Base;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using WarehouseManagement.DAL.Contract;
using WarehouseManagement.DAL.Implementation;
using WarehouseManagement.DAL.Models.Entity;
using WarehouseManagement.Model.Dto;
using WarehouseManagement.Model.Response.User;
using WarehouseManagement.Service.Contract;

namespace WarehouseManagement.Service.Implementation
{
    public class WarehouseService : IWarehouseService
    {
        private readonly IWarehouseRepository _warehouseRepository;
        private readonly IMapper _mapper;
        private IHttpContextAccessor _httpContextAccessor;

        public WarehouseService(IWarehouseRepository warehouseRepository, IMapper mapper, IHttpContextAccessor httpContextAccessor)
        {
            _warehouseRepository = warehouseRepository;
            _mapper = mapper;
            _httpContextAccessor = httpContextAccessor;
        }

        public AppResponse<WarehouseDto> CreateWarehouse(WarehouseDto request)
        {
            var result = new AppResponse<WarehouseDto>();
            try
            {
                var UserName = ClaimHelper.GetClainByName(_httpContextAccessor, "UserName");
                if (UserName == null)
                {
                    return result.BuildError("Cannot find Account by this user");
                }
                var warehouse = new Warehouse();
                warehouse = _mapper.Map<Warehouse>(request);
                warehouse.Id = Guid.NewGuid();
                warehouse.CreatedBy = UserName;
                _warehouseRepository.Add(warehouse);

                request.Id= warehouse.Id;
                result.IsSuccess = true;
                result.Data = request;
                return result;
            }
            catch (Exception ex)
            {
                result.IsSuccess = false;
                result.Message = ex.Message + " " + ex.StackTrace;
                return result;
            }
        }

        public AppResponse<string> DeleteWarehouse(Guid Id)
        {
            var result = new AppResponse<string>();
            try
            {
                var warehouse = new Warehouse();
                warehouse = _warehouseRepository.Get(Id);
                warehouse.IsDeleted = true;

                _warehouseRepository.Edit(warehouse);

                result.IsSuccess = true;
                result.Data = "Delete Sucessfuly";
                return result;
            }
            catch (Exception ex)
            {
                result.IsSuccess = false;
                result.Message = ex.Message + ":" + ex.StackTrace;
                return result;

            }
        }

        public AppResponse<WarehouseDto> EditWarehouse(WarehouseDto? request)
        {
            var result = new AppResponse<WarehouseDto>();
            try
            {
                var warehouse = new Warehouse();
                if (request.Id == null)
                {
                    result.IsSuccess = false;
                    result.Message = "Id cannot be null";
                    return result;
                }
                warehouse = _warehouseRepository.Get(request.Id.Value);
                warehouse.Name = request.Name;
                warehouse.Address = request.Address;
                warehouse.Managent = request.Managent;
                //budgetcat.Id = Guid.NewGuid();
                _warehouseRepository.Edit(warehouse);

                result.IsSuccess = true;
                result.Data = request;
                return result;
            }
            catch (Exception ex)
            {
                result.IsSuccess = false;
                result.Message = ex.Message + ":" + ex.StackTrace;
                return result;

            }
        }

        public AppResponse<List<WarehouseDto>> GetAllWarehouse()
        {
            var result = new AppResponse<List<WarehouseDto>>();
            //string userId = "";
            try
            {
                var query = _warehouseRepository.GetAll();
                var list = query.Select(m => new WarehouseDto
                {
                    Name = m.Name,
                    Address = m.Address,
                    Managent = m.Managent,
                    Email = m.Email,
                    Id = m.Id,
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

        public AppResponse<WarehouseDto> GetWarehouseById(Guid Id)
        {
            var result = new AppResponse<WarehouseDto>();
            try
            {
                var warehouse = _warehouseRepository.Get(Id);
                var data = _mapper.Map<WarehouseDto>(warehouse);
                result.IsSuccess = true;
                result.Data = data;
                return result;
            }
            catch (Exception ex)
            {
                result.IsSuccess = false;
                result.Message = ex.Message + " " + ex.StackTrace;
                return result;

            }
        }
        //public async Task<AppResponse<SearchUserResponse>> Search(SearchRequest request)
        //{
        //    var result = new AppResponse<SearchUserResponse>();
        //    try
        //    {
        //        var query = BuildFilterExpression(request.Filters);
        //        var numOfRecords = _userManagementRepository.CountRecordsByPredicate(query);

        //        var users = _userManagementRepository.FindByPredicate(query);
        //        int pageIndex = request.PageIndex ?? 1;
        //        int pageSize = request.PageSize ?? 1;
        //        int startIndex = (pageIndex - 1) * (int)pageSize;
        //        var UserList = users.Skip(startIndex).Take(pageSize).ToList();
        //        var dtoList = _mapper.Map<List<UserModel>>(UserList);
        //        if (dtoList != null && dtoList.Count > 0)
        //        {
        //            for (int i = 0; i < UserList.Count; i++)
        //            {
        //                var dtouser = dtoList[i];
        //                var identityUser = UserList[i];
        //                dtouser.Role = (await _userManager.GetRolesAsync(identityUser)).First();
        //            }
        //        }
        //        var searchUserResult = new SearchUserResponse
        //        {
        //            TotalRows = numOfRecords,
        //            TotalPages = CalculateNumOfPages(numOfRecords, pageSize),
        //            CurrentPage = pageIndex,
        //            Data = dtoList,
        //        };

        //        result.Data = searchUserResult;
        //        result.IsSuccess = true;

        //        return result;

        //    }
        //    catch (Exception ex)
        //    {

        //        return result.BuildError(ex.ToString());
        //    }
        //}


        //private ExpressionStarter<IdentityUser> BuildFilterExpression(IList<Filter> Filters)
        //{
        //    try
        //    {
        //        var predicate = PredicateBuilder.New<IdentityUser>(true);

        //        foreach (var filter in Filters)
        //        {
        //            switch (filter.FieldName)
        //            {
        //                case "Email":
        //                    predicate = predicate.And(m => m.Email.Equals(filter.Value));
        //                    break;

        //                default:
        //                    break;
        //            }
        //        }
        //        return predicate;
        //    }
        //    catch (Exception)
        //    {

        //        throw;
        //    }
        //}
    }
}
