using System.Data;
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
	public class InboundReceiptService : IInboundReceiptService
    {
        private readonly IInboundReceiptRepository _inboundReceiptRepository;
        private IMapper _mapper;
        private ISupplierRepository _supplierRepository;
        private IWarehouseRepository _warehouseRepository;
        private IImportProductRepository _importProductRepository;
        private IHttpContextAccessor _httpContextAccessor;
        private IProductRemainingRepository _productRemainingRepository;

        public InboundReceiptService(IInboundReceiptRepository inboundReceiptRepository, IMapper mapper,
            ISupplierRepository supplierRepository, IWarehouseRepository warehouseRepository, IHttpContextAccessor httpContextAccessor
            , IImportProductRepository importProductRepository, IProductRemainingRepository productRemainingRepository)
        {
            _inboundReceiptRepository = inboundReceiptRepository;
            _mapper = mapper;
            _supplierRepository = supplierRepository;
            _warehouseRepository = warehouseRepository;
            _httpContextAccessor = httpContextAccessor;
            _importProductRepository = importProductRepository;
            _productRemainingRepository = productRemainingRepository;
        }

        public AppResponse<InboundReceiptDto> CreateInboundReceipt(InboundReceiptDto request)
        {
            var result = new AppResponse<InboundReceiptDto>();
            try
            {
                var UserName = ClaimHelper.GetClainByName(_httpContextAccessor, "UserName");
                if (UserName == null)
                {
                    return result.BuildError("Cannot find Account by this user");
                }
                if (request.SupplierId == null)
                {
                    return result.BuildError("supplier cannot be null");
                }
                else if (request.WarehouseId == null)
                {
                    return result.BuildError("warehouse cannot be null");
                }
                var supplier =_supplierRepository.FindBy(x=>x.Id ==  request.SupplierId && x.IsDeleted == false);
                if (supplier.Count() == 0)
                {
                    return result.BuildError("Cannot find supplier");
                }
                var warehouse = _warehouseRepository.FindBy(x=>x.Id == request.WarehouseId && x.IsDeleted == false);
                if(warehouse.Count() == 0)
                {
                    return result.BuildError("Cannot find warehouse");
                }
                var listImportProduct= new List<ImportProduct>();
                //var listImportProductEdit= new List<ImportProduct>();
                //var listImportProductRemove= new List<ImportProduct>();

                var inboundReceipt = _mapper.Map<InboundReceipt>(request);
                    inboundReceipt.Warehouse = null;
                    inboundReceipt.Supplier = null;
                    inboundReceipt.CreatedBy = UserName;
                    
				_inboundReceiptRepository.Add(inboundReceipt);
				request.ListImportProductDto.ForEach(item =>
				{
					var product = new ImportProduct()
					{
						InboundReceiptId = inboundReceipt.Id,
						Quantity = item.Quantity,
						CreatedBy = UserName,
						CreatedOn = DateTime.UtcNow,
						ProductId = item.ProductId,
						SupplierId = item.SupplierId,
					};
					listImportProduct.Add(product);
				});
				_importProductRepository.AddRange(listImportProduct);
                request.Id = inboundReceipt.Id;
                    result.BuildResult(request);    



            }
            catch (Exception ex)
            {
                result.BuildError(ex.Message + "" + ex.StackTrace);
            }
            return result;
        }

        public AppResponse<string> DeleteInboundReceipt(int Id)
        {
            var result = new AppResponse<string>();
            try
            {
                var inboundReceipt =_inboundReceiptRepository.Get(Id);
                inboundReceipt.IsDeleted = true;
                _inboundReceiptRepository.Edit(inboundReceipt);

                var listImportProduct = _importProductRepository.GetAll().Where(x=>x.InboundReceiptId == inboundReceipt.Id).ToList();
                foreach(var product in listImportProduct)
                {
                    var productRemainming =_productRemainingRepository.FindBy(x=>x.ProductId == product.ProductId && x.SupplierId == product.SupplierId).First();
                    productRemainming.Quantity -= product.Quantity;
                    _productRemainingRepository.Edit(productRemainming);
                }

                result.BuildResult("đã xóa");
            }
            catch (Exception ex)
            {
                result.BuildError(ex.Message + "" + ex.StackTrace);
            }
            return result;
        }

        public AppResponse<InboundReceiptDto> EditInboundReceipt(InboundReceiptDto request)
        {
            var result = new AppResponse<InboundReceiptDto>();
            try
            {
                var inboundReceipt = _inboundReceiptRepository.Get(request.Id.Value);
                inboundReceipt.WarehouseId = request.WarehouseId;
                inboundReceipt.SupplierId = request.SupplierId;
                inboundReceipt.ModifiedOn = DateTime.UtcNow;
                _inboundReceiptRepository.Edit(inboundReceipt);
                result.BuildResult(request);
            }
            catch (Exception ex)
            {
                result.BuildError(ex.Message + "" + ex.StackTrace);
            }
            return result;
        }

		public AppResponse<List<InboundReceiptDto>> GetAllInboundReceipt()
        {
            var result = new AppResponse<List<InboundReceiptDto>>();
            try
            {
                var query = _inboundReceiptRepository.GetAll()
                    .Include(x=>x.Supplier)
                    .Include(x=>x.Supplier);
                var list = query.Select(x=> new InboundReceiptDto
                    {
                        Id = x.Id,
                        SupplierId = x.SupplierId,
                        SupplierName = x.Supplier.Name,
                        WarehouseId = x.WarehouseId,
                        WarehouseName = x.Warehouse.Name,
                        CreatedOn = TimeZoneInfo.ConvertTimeBySystemTimeZoneId((DateTime)x.CreatedOn, TimeZoneInfo.Local.Id),
                    }).ToList();

                result.BuildResult(list);
            }
            catch (Exception ex)
            {
                result.BuildError(ex.Message + "" + ex.StackTrace);
            }
            return result;
        }

        public AppResponse<InboundReceiptDto> GetInboundReceipt(int Id)
        {
            var result = new AppResponse<InboundReceiptDto>();
            try
            {
                var inboundReceipt = _inboundReceiptRepository.FindBy(x=>x.Id == Id)
                    .Include(x=>x.Warehouse)
                    .Include(x=>x.Supplier);
                var data = inboundReceipt.Select(x=>new InboundReceiptDto
                {
                    Id = x.Id,
                    SupplierId = x.SupplierId,
                    SupplierName = x.Supplier.Name,
                    WarehouseId = x.WarehouseId,
                    WarehouseName = x.Warehouse.Name,
                    CreatedOn = x.CreatedOn
                }).First();
                result.BuildResult(data);
            }
            catch (Exception ex)
            {
                result.BuildError(ex.Message + "" + ex.StackTrace);
            }
            return result;
        }
		public AppResponse<SearchResponse<InboundReceiptDto>> Search(SearchRequest request)
		{
			var result = new AppResponse<SearchResponse<InboundReceiptDto>>();
			try
			{
				var query = BuildFilterExpression(request.Filters);
				var numOfRecords = _inboundReceiptRepository.CountRecordsByPredicate(query);
				var model = _inboundReceiptRepository.FindByPredicate(query)
					.Include(x => x.Supplier)
					.Include(x => x.Warehouse);
				int pageIndex = request.PageIndex ?? 1;
				int pageSize = request.PageSize ?? 1;
				int startIndex = (pageIndex - 1) * (int)pageSize;
				var List = model.Skip(startIndex).Take(pageSize)
					.Select(x => new InboundReceiptDto
					{
						Id = x.Id,
						SupplierId = x.SupplierId,
                        SupplierName = x.Supplier.Name,
                        WarehouseId = x.WarehouseId,
                        WarehouseName = x.Warehouse.Name,
                        CreatedOn = x.CreatedOn,
					})
					.ToList();


				var searchUserResult = new SearchResponse<InboundReceiptDto>
				{
					TotalRows = numOfRecords,
					TotalPages = CalculateNumOfPages(numOfRecords, pageSize),
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
		private ExpressionStarter<InboundReceipt> BuildFilterExpression(IList<Filter> Filters)
		{
			try
			{
				var predicate = PredicateBuilder.New<InboundReceipt>(true);
                if (Filters!=null)
				foreach (var filter in Filters)
				{
					switch (filter.FieldName)
					{
						case "SupplierId":
							predicate = predicate.And(m => m.Supplier.Name.Equals((filter.Value)));
							break;
						case "WarehouseId":
							predicate = predicate.And(m => m.Warehouse.Name.Equals((filter.Value)));
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
                        case "Month":
                            {
								var day = DateTime.Parse(filter.Value);
								//if (filter.Value!="")
                                predicate = predicate.And(m => m.CreatedOn.Value.Month.Equals(day.Month) && m.CreatedOn.Value.Year.Equals(day.Year));
                            }
                            break;
                        case "Day":
                            {
                                var day =  DateTime.Parse(filter.Value);
								//if (filter.Value != "")
									predicate = predicate.And(m => m.CreatedOn.Value.Day.Equals(day.Day) && m.CreatedOn.Value.Month.Equals(day.Month) && m.CreatedOn.Value.Year.Equals(day.Year));
							}
                            break;
                        case "Year":
                            {
								var day = DateTime.Parse(filter.Value);
								//if (filter.Value != "")
								predicate = predicate.And(m => m.CreatedOn.Value.Year.Equals(day.Year));
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
