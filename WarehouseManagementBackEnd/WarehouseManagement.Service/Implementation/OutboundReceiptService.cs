using System.Data.Entity;
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
	public class OutboundReceiptService:IOutboundReceiptService
    {
        private IOutboundReceiptRepository _outboundReceiptRepository;
        private IMapper _mapper;
        private IWarehouseRepository _warehouseRepository;
        private IHttpContextAccessor _httpContextAccessor;
        private IExportProductService _exportProductService;

        public OutboundReceiptService(IOutboundReceiptRepository outboundReceiptRepository,
            IMapper mapper, IWarehouseRepository warehouseRepository, IHttpContextAccessor httpContextAccessory,
            IExportProductService exportProductService)
        {
            _outboundReceiptRepository = outboundReceiptRepository;
            _mapper = mapper;
            _warehouseRepository = warehouseRepository;
            _httpContextAccessor = httpContextAccessory;
            _exportProductService = exportProductService;
        }

        public AppResponse<OutboundReceiptDto> CreateOutboundReceipt(OutboundReceiptDto request)
        {
            var result = new AppResponse<OutboundReceiptDto>();
            try
            {
                var UserName = ClaimHelper.GetClainByName(_httpContextAccessor, "UserName");
                if (UserName == null)
                {
                    return result.BuildError("Cannot find Account by this user");
                }
                if (request.WarehouseId == null)
                {
                    return result.BuildError("warehouse cannot be null");
                }
                var warehouse =_warehouseRepository.FindBy(x=>x.Id == request.WarehouseId && x.IsDeleted ==false);
                if(warehouse.Count() == 0)
                {
                    return result.BuildError("Cannot find warehouse");
                }
                    var outboundReceipt = _mapper.Map<OutboundReceipt>(request);
                    outboundReceipt.Warehouse = null;
                    outboundReceipt.Id = Guid.NewGuid();
                    _outboundReceiptRepository.Add(outboundReceipt);
                request.ListExportProductDtos.ForEach(item => _exportProductService.CreateExportProduct(item));

                    request.Id = outboundReceipt.Id;
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

        public AppResponse<string> DeleteOutboundReceipt(Guid Id)
        {
            var result = new AppResponse<string>();
            try
            {
                var outboundReceipt = _outboundReceiptRepository.Get(Id);
                outboundReceipt.IsDeleted = true;

                _outboundReceiptRepository.Edit(outboundReceipt);

                result.IsSuccess = true;
                result.Data = "đã xóa";
                return result;
            }
            catch (Exception ex)
            {
                result.IsSuccess = false;
                result.Message = ex.Message + " " + ex.StackTrace;
                return result;
            }
        }

        public AppResponse<OutboundReceiptDto> EditOutbountReceipt(OutboundReceiptDto request)
        {
            var result = new AppResponse<OutboundReceiptDto>();
            try
            {
                var outboundReceipt = _outboundReceiptRepository.Get(request.Id.Value);
                outboundReceipt.WarehouseId = request.WarehouseId;
                outboundReceipt.To = request.To;

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

        public AppResponse<List<OutboundReceiptDto>> GetAllOutboundReceipt()
        {
            var result = new AppResponse<List<OutboundReceiptDto>>();
            try
            {
                var query = _outboundReceiptRepository.GetAll()
                    .Include(x => x.Warehouse);
                var list = query.Select(m => new OutboundReceiptDto
                {
                    Id = m.Id,
                    To = m.To,
                    WarehouseName = m.Warehouse.Name,
                    WarehouseId = m.Id,

                }).ToList();

                result.Data = list;
                result.IsSuccess = true;
                return result;
            }
            catch (Exception ex)
            {
                result.IsSuccess = false;
                result.Message = ex.Message + " " + ex.StackTrace;
                return result;
            }
        }

        public AppResponse<OutboundReceiptDto> GetOutboundReceipt(Guid Id)
        {
            var result = new AppResponse<OutboundReceiptDto>();
            try
            {
                var outboundReceipt = _outboundReceiptRepository.FindById(Id);
                var data = new OutboundReceiptDto();
                data.Id = outboundReceipt.Id;
                data.To = outboundReceipt.To;
                data.WarehouseId = outboundReceipt.WarehouseId;
                data.WarehouseName = outboundReceipt.Warehouse.Name;

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
		public AppResponse<SearchResponse<OutboundReceiptDto>> Search(SearchRequest request)
		{
			var result = new AppResponse<SearchResponse<OutboundReceiptDto>>();
			try
			{
				var query = BuildFilterExpression(request.Filters);
				var numOfRecords = _outboundReceiptRepository.CountRecordsByPredicate(query);
				var product = _outboundReceiptRepository.FindByPredicate(query);
				int pageIndex = request.PageIndex ?? 1;
				int pageSize = request.PageSize ?? 1;
				int startIndex = (pageIndex - 1) * (int)pageSize;
				var ProductList = product.Skip(startIndex).Take(pageSize)
					.Select(x => new OutboundReceiptDto
					{
						Id = x.Id,
                        To = x.To,
                        WarehouseId = x.WarehouseId,
                        WarehouseName = x.Warehouse.Name
					})
					.ToList();


				var searchUserResult = new SearchResponse<OutboundReceiptDto>
				{
					TotalRows = 0,
					TotalPages = CalculateNumOfPages(0, pageSize),
					CurrentPage = pageIndex,
					Data = ProductList,
				};
				result.BuildResult(searchUserResult);
			}
			catch (Exception ex)
			{
				result.BuildError(ex.Message);
			}
			return result;
		}
		private ExpressionStarter<OutboundReceipt> BuildFilterExpression(IList<Filter> Filters)
		{
			try
			{
				var predicate = PredicateBuilder.New<OutboundReceipt>(true);

				foreach (var filter in Filters)
				{
					switch (filter.FieldName)
					{
						case "WarehouseName":
							predicate = predicate.And(m => m.Warehouse.Name.Contains(filter.Value));
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
