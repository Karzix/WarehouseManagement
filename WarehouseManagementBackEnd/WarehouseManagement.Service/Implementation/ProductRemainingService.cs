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
	public class ProductRemainingService: IProductRemainingService
    {
        private readonly IProductRemainingRepository _productRemainingRepository;
        private IMapper _mapper;
        private IProductRepository _productRepository;
        private IWarehouseRepository _warehouseRepository;
        private IHttpContextAccessor _httpContextAccessor;

        public ProductRemainingService(IProductRemainingRepository productRemainingRepository,
            IMapper mapper, IProductRepository productRepository, IWarehouseRepository warehouseRepository, IHttpContextAccessor httpContextAccessor)
        {
            _productRemainingRepository = productRemainingRepository;
            _mapper = mapper;
            _productRepository = productRepository;
            _warehouseRepository = warehouseRepository;
            _httpContextAccessor = httpContextAccessor;
        }

        public AppResponse<ProductRemainingDto> CreateProductRemaining(ProductRemainingDto request)
        {
            var result = new AppResponse<ProductRemainingDto>();
            try
            {
                var UserName = ClaimHelper.GetClainByName(_httpContextAccessor, "UserName");
                if (UserName == null)
                {
                    return result.BuildError("Cannot find Account by this user");
                }
                if (request.ProductId == null)
                {
                    return result.BuildError("product cannot null");
                }
                var product = _productRepository.FindBy(x=>x.Id == request.ProductId && x.IsDeleted == false);
                if (product.Count() == 0)
                {
                    return result.BuildError("Cannot find product");
                }
                if(request.WarehouseId== null)
                {
                    return result.BuildError("warehouse cannot null");
                }
                var warehouse =_warehouseRepository.FindBy(x=>x.Id == request.WarehouseId && x.IsDeleted == false);
                if(warehouse.Count() == 0)
                {
                    return result.BuildError("Cannot find warehouse");
                }
                    var productREmaining = _mapper.Map<ProductRemaining>(request);
                    productREmaining.Product = null;
                    productREmaining.Warehouse = null;
                    productREmaining.CreatedBy = UserName;
                    _productRemainingRepository.Add(productREmaining);
                    request.Id = productREmaining.Id;
                    result.IsSuccess = true;
                    result.Data = request;
            }
            catch (Exception ex)
            {
                result.IsSuccess = false;
                result.Message = ex.Message+ " " + ex.StackTrace;
            }
            return result;
        }

        public AppResponse<string> DeleteProductRemaining(int Id)
        {
            var result = new AppResponse<string>();
            try
            {
                var productRemaining = _productRemainingRepository.Get(Id);
                productRemaining.IsDeleted = true;
                _productRemainingRepository.Edit(productRemaining);
                result.IsSuccess = true;
                result.Data = "đã xóa";
            }
            catch (Exception ex)
            {
                result.IsSuccess = false;
                result.Message = ex.Message + " " + ex.StackTrace;
            }
            return result;
        }

        public AppResponse<ProductRemainingDto> EditProductRemaining(ProductRemainingDto request)
        {
            var result = new AppResponse<ProductRemainingDto>();
            try
            {
                var productRemaining = _productRemainingRepository.Get(request.Id.Value);
                productRemaining.ProductId = request.ProductId;
                productRemaining.Quantity = request.Quantity;
                productRemaining.WarehouseId = request.WarehouseId;
                productRemaining.ModifiedOn = DateTime.UtcNow;
                productRemaining.Id = (int)request.Id;

                result.IsSuccess = true;
                result.Data = request;


            }
            catch (Exception ex)
            {
                result.IsSuccess = false;
                result.Message = ex.Message + " " + ex.StackTrace;
            }
            return result;
        }

        public AppResponse<List<ProductRemainingDto>> GetAllProductRemaining()
        {
            var result = new AppResponse<List<ProductRemainingDto>>();
            try
            {
                var query = _productRemainingRepository.GetAll()
                    .Include(x => x.Product)
                    .Include(x => x.Warehouse);
                var list = query.Select(m => new ProductRemainingDto
                {
                    Id = m.Id,
                    WarehouseId = m.WarehouseId,
                    WarehouseName = m.Warehouse.Name,
                    ProductId = m.ProductId,
                    ProductName = m.Product.Name,
                    Quantity = m.Quantity,
                })
                .ToList();

                result.Data = list;
                result.IsSuccess = true;
                return result;
            }
            catch (Exception ex)
            {
                result.IsSuccess = false;
                result.Message = ex.Message + " " + ex.StackTrace;
            }
            return result;
        }

        public AppResponse<ProductRemainingDto> GetProductRemaining(int Id)
        {
            var result = new AppResponse<ProductRemainingDto>();
            try
            {
                var productRemaining = _productRemainingRepository.FindById(Id);

                var data = _mapper.Map<ProductRemainingDto>(productRemaining);

                result.Data = data;
                result.IsSuccess = true;
            }
            catch (Exception ex)
            {
                result.IsSuccess = false;
                result.Message = ex.Message + " " + ex.StackTrace;
            }
            return result;
        }
		public AppResponse<SearchResponse<ProductRemainingDto>> Search(SearchRequest request)
		{
			var result = new AppResponse<SearchResponse<ProductRemainingDto>>();
			try
			{
				var query = BuildFilterExpression(request.Filters);
				var numOfRecords = _productRemainingRepository.CountRecordsByPredicate(query);
				var model = _productRemainingRepository.FindByPredicate(query)
                    .Include(x=>x.Product)
                    .Include(x=>x.Warehouse);
				int pageIndex = request.PageIndex ?? 1;
				int pageSize = request.PageSize ?? 1;
				int startIndex = (pageIndex - 1) * (int)pageSize;
                var List = model.Skip(startIndex).Take(pageSize)
                    .Select(x => new ProductRemainingDto
                    {
                        Id = x.Id,
                        Quantity = x.Quantity,
                        ProductId = x.ProductId,
                        ProductName = x.Product.Name,
                        WarehouseId = x.WarehouseId,
                        WarehouseName = x.Warehouse.Name
                    })
					.ToList();


				var searchUserResult = new SearchResponse<ProductRemainingDto>
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
		private ExpressionStarter<ProductRemaining> BuildFilterExpression(IList<Filter> Filters)
		{
			try
			{
				var predicate = PredicateBuilder.New<ProductRemaining>(true);

				foreach (var filter in Filters)
				{
					switch (filter.FieldName)
					{
						case "ProductName":
							predicate = predicate.And(m => m.Product.Name.Contains(filter.Value));
							break;
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
