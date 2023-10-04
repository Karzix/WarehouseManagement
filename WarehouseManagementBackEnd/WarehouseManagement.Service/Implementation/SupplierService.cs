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
	public class SupplierService : ISupplierService
    {
        private readonly ISupplierRepository _supplierRepository;
        private readonly IMapper _mapper;
        private IHttpContextAccessor _httpContextAccessor;

        public SupplierService(ISupplierRepository supplierRepository, IMapper mapper, IHttpContextAccessor httpContextAccessor)
        {
            _supplierRepository = supplierRepository;
            _mapper = mapper;
            _httpContextAccessor = httpContextAccessor; 
        }

        public AppResponse<SupplierDto> CreateSupplier(SupplierDto supplier)
        {
            var result = new AppResponse<SupplierDto>();
            try
            {
                var UserName = ClaimHelper.GetClainByName(_httpContextAccessor, "UserName");
                if (UserName == null)
                {
                    return result.BuildError("Cannot find Account by this user");
                }

                var request = _mapper.Map<Supplier>(supplier);
                request.Id =  Guid.NewGuid();
                request.CreatedBy = UserName;
                _supplierRepository.Add(request);

                supplier.Id = request.Id;
                result.IsSuccess = true;
                result.Data = supplier;
                return result;  
            }
            catch (Exception ex)
            {
                result.IsSuccess = false;
                result.Message = ex.Message + " " + ex.StackTrace;
                return result;
            }
        }

        public AppResponse<string> DeleteSupplier(Guid Id)
        {
            var result =  new AppResponse<string>();
            try
            {
                var request = _supplierRepository.Get(Id);
                request.IsDeleted = true;
                _supplierRepository.Edit(request);

                result.IsSuccess = true;
                result.Data = "Dã xóa";
                return result;
            }
            catch (Exception ex)
            {
                result.IsSuccess=false;
                result.Message = ex.Message + " " + ex.StackTrace;
                return result;
            }
        }

        public AppResponse<SupplierDto> EditSupplier(SupplierDto supplier)
        {
            var result = new AppResponse<SupplierDto>();
            try
            {
                var request = _supplierRepository.Get(supplier.Id.Value);
                request.Name = supplier.Name;
                request.Email = supplier.Email;
                request.ModifiedOn = DateTime.UtcNow;
                _supplierRepository.Edit(request);

                result.IsSuccess = true;
                result.Data = supplier;
                return result;
            }
            catch(Exception ex)
            {
                result.IsSuccess = false;
                result.Message = ex.Message + " " + ex.StackTrace;
                return result;
            }
        }

        public AppResponse<List<SupplierDto>> GetAllSupplier()
        {
            var result = new AppResponse<List<SupplierDto>>();
            try
            {
                var query = _supplierRepository.GetAll();
                var list = query.Select(m => new SupplierDto
                {
                    Name = m.Name,
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

        public AppResponse<SupplierDto> GetSupplier(Guid Id)
        {
            var result =  new AppResponse<SupplierDto>();
            try
            {
                var query = _supplierRepository.Get(Id);
                var request = _mapper.Map<SupplierDto>(query);

                result.IsSuccess = true;
                result.Data = request;
                return result;
            }
            catch(Exception ex)
            {
                result.IsSuccess = false;
                result.Message = ex.Message + " " + ex.StackTrace;
                return result;
            }

        }
		public AppResponse<SearchResponse<SupplierDto>> Search(SearchRequest request)
		{
			var result = new AppResponse<SearchResponse<SupplierDto>>();
			try
			{
				var query = BuildFilterExpression(request.Filters);
				var numOfRecords = _supplierRepository.CountRecordsByPredicate(query);
				var model = _supplierRepository.FindByPredicate(query);
				int pageIndex = request.PageIndex ?? 1;
				int pageSize = request.PageSize ?? 1;
				int startIndex = (pageIndex - 1) * (int)pageSize;
				var List = model.Skip(startIndex).Take(pageSize)
					.Select(x => new SupplierDto
					{
						Id = x.Id,
						Name = x.Name,
                        Email = x.Email,
					})
					.ToList();


				var searchUserResult = new SearchResponse<SupplierDto>
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
		private ExpressionStarter<Supplier> BuildFilterExpression(IList<Filter> Filters)
		{
			try
			{
				var predicate = PredicateBuilder.New<Supplier>(true);

				foreach (var filter in Filters)
				{
					switch (filter.FieldName)
					{
						case "SupplierName":
							predicate = predicate.And(m => m.Name.Contains(filter.Value));
							break;
						case "IsDelete":
                            {
                                bool isDetete = false;
                                if(filter.Value == "True" || filter.Value == "true")
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
