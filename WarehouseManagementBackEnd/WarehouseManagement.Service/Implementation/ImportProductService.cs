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
	public class ImportProductService:IImportProductService
    {
        private IImportProductRepository _importProductRepository;
        private IMapper _mapper;
        private IInboundReceiptRepository _inboundReceiptRepository;
        private ISupplierRepository _supplierRepository;
        private IProductRepository _productRepository;
        private IHttpContextAccessor _httpContextAccessor;

        public ImportProductService(IImportProductRepository importProductRepository, IMapper mapper, IInboundReceiptRepository inboundReceiptRepository, ISupplierRepository supplierRepository, IProductRepository productRepository, IHttpContextAccessor httpContextAccessor)
        {
            _importProductRepository = importProductRepository;
            _mapper = mapper;
            _inboundReceiptRepository = inboundReceiptRepository;
            _supplierRepository = supplierRepository;
            _productRepository = productRepository;
            _httpContextAccessor = httpContextAccessor;
        }

        public AppResponse<ImportProductDto> CreateImportProduct(ImportProductDto requets)
        {
            var result = new AppResponse<ImportProductDto>();
            try
            {
                var UserName = ClaimHelper.GetClainByName(_httpContextAccessor, "UserName");
                if (UserName == null)
                {
                    return result.BuildError("Cannot find Account by this user");
                }
                if (requets.SupplierId == null)
                {
                    return result.BuildError("Supplier cannot null");
                }
                var supplier =_supplierRepository.FindBy(x=>x.Id == requets.SupplierId && x.IsDeleted == false);
                if(supplier.Count() == 0)
                {
                    return result.BuildError("Cannot find supplier");
                }
                if (requets.ProductId == null)
                {
                    return result.BuildError("Product cannot null");
                }
                var product = _productRepository.FindBy(x => x.Id == requets.ProductId && x.IsDeleted == false);
                if (product.Count() == 0)
                {
                    return result.BuildError("Cannot find product");
                }
                if (requets.InboundReceiptId == null)
                {
                    return result.BuildError("Inbound receipt cannot null");
                }
                var inboundReceipt =_inboundReceiptRepository.FindBy(x=>x.Id == requets.InboundReceiptId && x.IsDeleted == false);
                if(inboundReceipt.Count() == 0)
                {
                    return result.BuildError("Cannot find inbound receipt");
                }
                var importProduct = _mapper.Map<ImportProduct>(requets);
                importProduct.Supplier = null;
                importProduct.Product = null;
                importProduct.InboundReceipt = null;
                importProduct.CreatedBy = UserName;
                _importProductRepository.Add(importProduct);

                requets.Id = importProduct.Id;
                result.BuildResult(requets);
            }
            catch(Exception ex)
            {
                result.BuildError(ex.Message);
            }
            return result;
        }

        public AppResponse<string> DeleteImportProduct(int Id)
        {
            var result =  new AppResponse<string>();
            try
            {
                var importProduct = _importProductRepository.Get(Id);
                importProduct.IsDeleted = true;
                _importProductRepository.Edit(importProduct);
                result.BuildResult("đã xóa");
            }
            catch (Exception ex)
            {
                result.BuildError(ex.Message);
            }
            return result;
        }

        public AppResponse<ImportProductDto> EditImportProduct(ImportProductDto requets)
        {
            var result = new AppResponse<ImportProductDto>();
            try
            {
                var inportProduct = _importProductRepository.Get(requets.Id.Value);
                inportProduct.SupplierId = requets.SupplierId;
                inportProduct.ProductId = requets.ProductId;
                inportProduct.Quantity = requets.Quantity;
                inportProduct.InboundReceiptId = (int)requets.InboundReceiptId;
                inportProduct.ModifiedOn = DateTime.UtcNow;
            }
            catch (Exception ex)
            {
                result.BuildError(ex.Message);
            }
            return result;
        }

        public AppResponse<List<ImportProductDto>> GetAllImportProduct()
        {
            var result = new AppResponse<List<ImportProductDto>>();
            try
            {
                var query = _importProductRepository.GetAll()
                    .Include(x => x.Supplier)
                    .Include(x => x.Product);
                var list = query.Select(x=> new ImportProductDto
                {
                    Quantity = x.Quantity,
                    Id = x.Id,
                    InboundReceiptId = (int)x.InboundReceiptId,
                    ProductName = x.Product.Name,
                    SipplierName = x.Supplier.Name,
                    SupplierId = x.SupplierId,
                    ProductId = x.ProductId,
                    //CreatedOn = x.CreatedOn,
                }).ToList();

                result.BuildResult(list);
            }
            catch (Exception ex)
            {
                result.BuildError(ex.Message);
            }
            return result;
        }

        public AppResponse<ImportProductDto> GetImportProduct(int Id)
        {
            var result = new AppResponse<ImportProductDto>();
            try
            {
                var query = _importProductRepository.FindBy(x=>x.Id == Id)
                    .Include(x=>x.Supplier)
                    .Include(x=>x.Product);
                var data =  query.Select(x=> new ImportProductDto
                {
                    Quantity = x.Quantity,
                    Id = x.Id,
                    InboundReceiptId = (int)x.InboundReceiptId,
                    ProductId = x.ProductId,
                    ProductName = x.Product.Name,
                    SipplierName = x.Supplier.Name,
                    SupplierId = x.SupplierId,
                    //CreatedOn= x.CreatedOn,
                }).First();
                result.BuildResult(data);
            }
            catch (Exception ex)
            {
                result.BuildError(ex.Message);
            }
            return result;
        }
		public AppResponse<SearchResponse<ImportProductDto>> Search(SearchRequest request)
		{
			var result = new AppResponse<SearchResponse<ImportProductDto>>();
			try
			{
				var query = BuildFilterExpression(request.Filters);
				var numOfRecords = _importProductRepository.CountRecordsByPredicate(query);
				var model = _importProductRepository.FindByPredicate(query).OrderByDescending(x => x.CreatedOn)
                    .Include(x => x.Supplier)
					.Include(x => x.Product)
                    .Include(x=>x.InboundReceipt);
				int pageIndex = request.PageIndex ?? 1;
				int pageSize = request.PageSize ?? 1;
				int startIndex = (pageIndex - 1) * (int)pageSize;
				var List = model.Skip(startIndex).Take(pageSize)
					.Select(x => new ImportProductDto
					{
						Id = x.Id,
						SupplierId = x.SupplierId,
                        Quantity = x.Quantity,
                        InboundReceiptId = (int)x.InboundReceiptId,
                        ProductId = x.ProductId,
                        ProductName = x.Product.Name,
                        SipplierName = x.Supplier.Name,
					})
					.ToList();


				var searchUserResult = new SearchResponse<ImportProductDto>
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
		private ExpressionStarter<ImportProduct> BuildFilterExpression(IList<Filter> Filters)
		{
			try
			{
				var predicate = PredicateBuilder.New<ImportProduct>(true);
                if (Filters!=null)
				foreach (var filter in Filters)
				{
					switch (filter.FieldName)
					{
						case "SupplierName":
							predicate = predicate.And(m => m.Supplier.Name.Contains(filter.Value));
							break;
                        case "InboundReceiptId":
                            predicate = predicate.And(x=>x.InboundReceiptId.Equals(int.Parse(filter.Value)));
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
							case "Month":
								{
									var day = DateTime.Parse(filter.Value);
									//if (filter.Value!="")
									predicate = predicate.And(m => m.CreatedOn.Value.Month.Equals(day.Month) && m.CreatedOn.Value.Year.Equals(day.Year));
								}
								break;
							case "Day":
								{
									var day = DateTime.Parse(filter.Value);
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
