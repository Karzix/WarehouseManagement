using AutoMapper;
using LinqKit;
using MayNghien.Common.Helpers;
using MayNghien.Models.Request.Base;
using MayNghien.Models.Response.Base;
using Microsoft.AspNetCore.Http;
using WarehouseManagement.DAL.Contract;
using WarehouseManagement.DAL.Models.Entity;
using WarehouseManagement.Model.Dto;
using WarehouseManagement.Service.Contract;
using static Maynghien.Common.Helpers.SearchHelper;

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

        public AppResponse<string> DeleteWarehouse(int Id)
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
                warehouse.ModifiedOn = DateTime.UtcNow;
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

        public AppResponse<WarehouseDto> GetWarehouseById(int Id)
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
		public AppResponse<SearchResponse<WarehouseDto>> Search(SearchRequest request)
		{
			var result = new AppResponse<SearchResponse<WarehouseDto>>();
			try
			{
				var query = BuildFilterExpression(request.Filters);
				var numOfRecords = _warehouseRepository.CountRecordsByPredicate(query);
				var model = _warehouseRepository.FindByPredicate(query);
				int pageIndex = request.PageIndex ?? 1;
				int pageSize = request.PageSize ?? 1;
				int startIndex = (pageIndex - 1) * (int)pageSize;
				var List = model.Skip(startIndex).Take(pageSize)
					.Select(x => new WarehouseDto
					{
						Id = x.Id,
						Name = x.Name,
						Email = x.Email,
                        Address = x.Address,
                        Managent = x.Managent,
					})
					.ToList();


				var searchUserResult = new SearchResponse<WarehouseDto>
				{
					TotalRows = 0,
					TotalPages = CalculateNumOfPages(0, pageSize),
					CurrentPage = pageIndex,
					Data = List,
				};
				result.BuildResult(searchUserResult);
			}
			catch (Exception ex)
			{
				result.BuildError(ex.Message);
			}
			return result;
		}
		private ExpressionStarter<Warehouse> BuildFilterExpression(IList<Filter> Filters)
		{
			try
			{
				var predicate = PredicateBuilder.New<Warehouse>(true);
                if (Filters!=null)
				foreach (var filter in Filters)
				{
					switch (filter.FieldName)
					{
						case "Name":
							predicate = predicate.And(m => m.Name.Contains(filter.Value));
							break;
						case "IsDelete":
							{
								bool isDetete = false;
								if (filter.Value == "True" || filter.Value == "true")
								{
									isDetete = true;
								}
								predicate = predicate.And(m => m.IsDeleted == isDetete);
							}
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
   