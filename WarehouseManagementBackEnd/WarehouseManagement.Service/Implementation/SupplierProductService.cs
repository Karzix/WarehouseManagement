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
	public class SupplierProductService : ISupplierProductService
    {
        private readonly ISupplierProductRepository _supplierProductRepository;
        private readonly IMapper _mapper;
        private ISupplierRepository _supplierRepository;
        private IProductRepository _productRepository;
        private IHttpContextAccessor _httpContextAccessor;
        public SupplierProductService(ISupplierProductRepository supplierProductRepository, IMapper mapper,
            ISupplierRepository supplierRepository, IProductRepository productRepository, IHttpContextAccessor httpContextAccessor)
        {
            _supplierProductRepository = supplierProductRepository;
            _mapper = mapper;
            _supplierRepository = supplierRepository;
            _productRepository = productRepository;
            _httpContextAccessor = httpContextAccessor;
        }

        public AppResponse<SupplierProductDto> CreateSupplierProduct(SupplierProductDto request)
        {
            var result = new AppResponse<SupplierProductDto>();
            try
            {
                var UserName = ClaimHelper.GetClainByName(_httpContextAccessor, "UserName");
                if (UserName == null)
                {
                    return result.BuildError("Cannot find Account by this user");
                }

                var supplier = _supplierRepository.Get(request.SupplierId);
                var product =_productRepository.Get(request.ProductId);
                if (supplier == null && supplier.IsDeleted != false)
                {

                    return result.BuildError("supplier wrong!");
                }
                else if (product == null && product.IsDeleted != false)
                {
                    return result.BuildError("product wrong!");
                }
                else
                {
                    var supplierproduct = _mapper.Map<SupplierProduct>(request);
                    supplierproduct.Id = Guid.NewGuid();
                    supplierproduct.Product = product;
                    supplierproduct.Supplier = supplier;
                    supplierproduct.CreatedBy = UserName;
                    _supplierProductRepository.Add(supplierproduct);

                    request.Id = supplierproduct.Id;
                    result.IsSuccess = true;
                    result.Data = request;
                    return result;
                }
            }
            catch (Exception ex)
            {
                result.IsSuccess = false;
                result.Message = ex.Message + " " + ex.StackTrace;
                return result;
            }
        }

        public AppResponse<string> DeleteSupplierProduct(Guid Id)
        {
            var result = new AppResponse<string>();
            try
            {
                var supplierProduct = _supplierProductRepository.Get(Id);
                supplierProduct.IsDeleted = true;
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

        public AppResponse<SupplierProductDto> EditSupplierProduct(SupplierProductDto request)
        {
            var result = new AppResponse<SupplierProductDto>();
            try
            {
                var supplierPoduct = _supplierProductRepository.Get(request.Id.Value);
                supplierPoduct.SupplierId = request.SupplierId;
                supplierPoduct.ProductId = request.ProductId;
                supplierPoduct.ModifiedOn = DateTime.UtcNow;
                _supplierProductRepository.Edit(supplierPoduct);
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

        public AppResponse<List<SupplierProductDto>> GetAllSupplierProduct()
        {
            var result = new AppResponse<List<SupplierProductDto>>();
            try
            {
                var query = _supplierProductRepository.GetAll()
                    .Include(x=>x.Supplier)
                    .Include(x=>x.Product);
                var list = query.Select(m => new SupplierProductDto
                {
                    SupplierId = m.SupplierId,
                    ProductId = m.ProductId,
                    SupplierName = m.Supplier.Name,
                    ProductName = m.Product.Name,
                    Id = m.Id
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

        public AppResponse<SupplierProductDto> GetSupplierProduct(Guid Id)
        {
            var result = new AppResponse<SupplierProductDto>();
            try
            {
                var supplierProduct = _supplierProductRepository.FindById(Id);
                var data = new SupplierProductDto();
                data.SupplierName =supplierProduct.Supplier.Name;
                data.SupplierId = supplierProduct.SupplierId;
                data.ProductName = supplierProduct.Product.Name;
                data.ProductId =supplierProduct.ProductId;
                data.Id = supplierProduct.Id;
                result.Data = data;
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
        public AppResponse<SearchResponse<ProductDto>> Search(SearchRequest request)
        {
            var result =new AppResponse<SearchResponse<ProductDto>>();
            try
            {
                var query = BuildFilterExpression(request.Filters);
                var numOfRecords = _supplierProductRepository.CountRecordsByPredicate(query);
                var supplierProduct = _supplierProductRepository.FindByPredicate(query).Include(x=>x.Product);
                int pageIndex = request.PageIndex ?? 1;
                int pageSize = request.PageSize ?? 1;
                int startIndex = (pageIndex - 1) * (int)pageSize;
                var ProductList = supplierProduct.Skip(startIndex).Take(pageSize)
                    .Select(x=> new ProductDto
                    {
                        Quantity = x.Product.Quantity,
                        Description = x.Product.Description,
                        Id = x.ProductId,
                        Name = x.Product.Name,
                    })
                    .ToList();

                
                var searchUserResult = new SearchResponse<ProductDto>
                {
                    TotalRows = 0,
                    TotalPages = CalculateNumOfPages(0, pageSize),
                    CurrentPage = pageIndex,
                    Data = ProductList,
                };
                result.BuildResult(searchUserResult);
            }
            catch(Exception ex)
            {
                result.BuildError(ex.Message);
            }
            return result;
        }
        private ExpressionStarter<SupplierProduct> BuildFilterExpression(IList<Filter> Filters)
        {
            try
            {
                var predicate = PredicateBuilder.New<SupplierProduct>(true);

                foreach (var filter in Filters)
                {
                    switch (filter.FieldName)
                    {
                        case "SupplierId":
                            predicate = predicate.And(m => m.Supplier.Id.Equals(Guid.Parse(filter.Value)));
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
