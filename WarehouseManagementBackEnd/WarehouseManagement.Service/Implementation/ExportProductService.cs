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
	public class ExportProductService : IExportProductService
    {
        private readonly DAL.Contract.IExportProductRepository _repository;
        private readonly IMapper _mapper;
        private IOutboundReceiptRepository _outboundReceiptRepository;
        private ISupplierRepository _supplierRepository;
        private IProductRepository _productRepository;
        private IHttpContextAccessor _httpContextAccessor;

        public ExportProductService(IExportProductRepository repository, IMapper mapper, IOutboundReceiptRepository outboundReceiptRepository, ISupplierRepository supplierRepository, IProductRepository productRepository, IHttpContextAccessor httpContextAccessor)
        {
            _repository = repository;
            _mapper = mapper;
            _outboundReceiptRepository = outboundReceiptRepository;
            _supplierRepository = supplierRepository;
            _productRepository = productRepository;
            _httpContextAccessor = httpContextAccessor;
        }

        public AppResponse<ExportProductDto> CreateExportProduct(ExportProductDto request)
        {
            var result = new AppResponse<ExportProductDto>();
            try
            {
                var UserName = ClaimHelper.GetClainByName(_httpContextAccessor, "UserName");
                if (UserName == null)
                {
                    return result.BuildError("Cannot find Account by this user");
                }
                if (request.SupplierId == null)
                {
                    return result.BuildError("Supplier cannot null");
                }
                var supplier = _supplierRepository.FindBy(x => x.Id == request.SupplierId && x.IsDeleted == false);
                if (supplier.Count() == 0)
                {
                    return result.BuildError("Cannot find supplier");
                }
                if (request.ProductId == null)
                {
                    return result.BuildError("Product cannot null");
                }
                var product = _productRepository.FindBy(x => x.Id == request.SupplierId && x.IsDeleted == false);
                if (product.Count() == 0)
                {
                    return result.BuildError("Cannot find product");
                }
                if (request.OutboundReceiptId == null)
                {
                    return result.BuildError("Outbound receipt cannot null");
                }
                var outboundReceipt =_outboundReceiptRepository.FindBy(x=>x.Id ==request.OutboundReceiptId && x.IsDeleted==false);
                if(outboundReceipt.Count() == 0)
                {
                    return result.BuildError("Cannot find outbound receipt");
                }
                var exportProduct = _mapper.Map<ExportProduct>(request);
                exportProduct.CreatedBy = UserName;
                _repository.Add(exportProduct);

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

        public AppResponse<string> DeleteExportProduct(int Id)
        {
            var result = new AppResponse<string>();
            try
            {
                var exportProduct = _repository.Get(Id);

                exportProduct.IsDeleted = true;
                _repository.Edit(exportProduct);
                result.IsSuccess = true;
                result.Data = "Đã xóa";
                return result;
            }
            catch (Exception ex)
            {
                result.IsSuccess = false;
                result.Message = ex.Message + " " + ex.StackTrace;
                return result;
            }
        }

        public AppResponse<ExportProductDto> EditExportProduct(ExportProductDto request)
        {
            var result = new AppResponse<ExportProductDto>();
            try
            {
                var exportProduct = new ExportProduct();
                exportProduct.Id = (int)request.Id;
                exportProduct.SupplierId = request.SupplierId;
                exportProduct.ProductId = request.ProductId;
                exportProduct.OutboundReceiptId = (int)request.OutboundReceiptId;
                exportProduct.Quantity = request.Quantity;
                exportProduct.ModifiedOn = DateTime.UtcNow;
                _repository.Edit(exportProduct);
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

        public AppResponse<List<ExportProductDto>> GetAllExportProduct()
        {
            var result = new AppResponse<List<ExportProductDto>>();
            try
            {
                var query = _repository.GetAll()
                    .Include(s=> s.Supplier)
                    .Include(s=>s.Product);
                var list = query.Select(m => new ExportProductDto
                {
                    Quantity = m.Quantity,
                    SupplierId = m.SupplierId,
                    ProductId = m.ProductId,
                    OutboundReceiptId = m.OutboundReceiptId,
                    Id = m.Id,
                    SupplierName = m.Supplier.Name,
                    ProductName = m.Product.Name,
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

        public AppResponse<ExportProductDto> GetExportProduct(int Id)
        {
            var result = new AppResponse<ExportProductDto>();
            try
            {
                var exportProduct = _repository.FindById(Id);
                var exportProductDto = _mapper.Map<ExportProductDto>(exportProduct);
                exportProductDto.SupplierName = exportProduct.Supplier.Name;
                exportProductDto.ProductName = exportProduct.Product.Name;

                result.IsSuccess = true;
                result.Data = exportProductDto;
                return result;
            }
            catch (Exception ex)
            {
                result.IsSuccess = false;
                result.Message = ex.Message + " " + ex.StackTrace;
                return result;
            }
        }
		public AppResponse<SearchResponse<ExportProductDto>> Search(SearchRequest request)
		{
			var result = new AppResponse<SearchResponse<ExportProductDto>>();
			try
			{
				var query = BuildFilterExpression(request.Filters);
				var numOfRecords = _repository.CountRecordsByPredicate(query);
				var model = _repository.FindByPredicate(query).OrderByDescending(x => x.CreatedOn)
					.Include(x => x.Supplier)
					.Include(x => x.Product)
					.Include(x => x.OutboundReceipt);
				int pageIndex = request.PageIndex ?? 1;
				int pageSize = request.PageSize ?? 1;
				int startIndex = (pageIndex - 1) * (int)pageSize;
				var List = model.Skip(startIndex).Take(pageSize)
					.Select(x => new ExportProductDto
					{
						Id = x.Id,
						SupplierId = x.SupplierId,
                        Quantity = x.Quantity,
                        OutboundReceiptId = x.OutboundReceiptId,
                        ProductId = x.ProductId,
                        ProductName = x.Product.Name,
                        SupplierName = x.Supplier.Name,
					})
					.ToList();


				var searchUserResult = new SearchResponse<ExportProductDto>
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
		private ExpressionStarter<ExportProduct> BuildFilterExpression(IList<Filter> Filters)
		{
			try
			{
				var predicate = PredicateBuilder.New<ExportProduct>(true);
                if (Filters!=null)
				foreach (var filter in Filters)
				{
					switch (filter.FieldName)
					{
						case "SupplierName":
							predicate = predicate.And(m => m.Supplier.Name.Contains(filter.Value));
							break;
						case "OutboundReceiptId":
							predicate = predicate.And(x => x.OutboundReceiptId.Equals(int.Parse(filter.Value)));
							break;
						case "ProductName":
							predicate = predicate.And(x => x.Product.Name.Contains(filter.Value));
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
                predicate = predicate.And(m => m.IsDeleted == false);
                return predicate;
			}
			catch (Exception)
			{
				throw;
			}
		}
	}
}
