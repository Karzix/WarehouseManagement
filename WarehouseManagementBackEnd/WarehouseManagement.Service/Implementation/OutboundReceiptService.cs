﻿using System.Data.Entity;
using AutoMapper;
using LinqKit;
using MayNghien.Common.Helpers;
using MayNghien.Models.Request.Base;
using MayNghien.Models.Response.Base;
using Microsoft.AspNetCore.Http;
using WarehouseManagement.DAL.Contract;
using WarehouseManagement.DAL.Implementation;
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
        private IExportProductRepository _exportProductRepository;
        private IProductRemainingRepository _productRemainingRepository;

        public OutboundReceiptService(IOutboundReceiptRepository outboundReceiptRepository,
            IMapper mapper, IWarehouseRepository warehouseRepository, IHttpContextAccessor httpContextAccessory,
			IExportProductRepository exportProductRepository, IProductRemainingRepository productRemainingRepository)
        {
            _outboundReceiptRepository = outboundReceiptRepository;
            _mapper = mapper;
            _warehouseRepository = warehouseRepository;
            _httpContextAccessor = httpContextAccessory;
			_exportProductRepository = exportProductRepository;
            _productRemainingRepository = productRemainingRepository;
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
                    _outboundReceiptRepository.Add(outboundReceipt);
				var list = new List<ExportProduct>();
				request.ListExportProductDtos.ForEach(item =>
				{
					var product = new ExportProduct()
					{
						OutboundReceiptId = outboundReceipt.Id,
						Quantity = item.Quantity,
						CreatedBy = UserName,
						CreatedOn = DateTime.UtcNow,
						ProductId = item.ProductId,
						SupplierId = item.SupplierId,
					};
					list.Add(product);
                    var productRemainming = _productRemainingRepository.FindBy(x => x.ProductId == item.ProductId && x.WarehouseId == request.WarehouseId && x.SupplierId == item.SupplierId).First();
                    productRemainming.Quantity -= item.Quantity;
                    _productRemainingRepository.Edit(productRemainming);
				});


				_exportProductRepository.AddRange(list);
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

        public AppResponse<string> DeleteOutboundReceipt(int Id)
        {
            var result = new AppResponse<string>();
            try
            {
                var outboundReceipt = _outboundReceiptRepository.Get(Id);
                outboundReceipt.IsDeleted = true;

                _outboundReceiptRepository.Edit(outboundReceipt);
                var listExportProduct = _exportProductRepository.GetAll().Where(x => x.OutboundReceiptId == outboundReceipt.Id).ToList();
                foreach (var product in listExportProduct)
                {
                    var productRemainming = _productRemainingRepository.FindBy(x => x.ProductId == product.ProductId && x.SupplierId == product.SupplierId).First();
                    productRemainming.Quantity += product.Quantity;
                    _productRemainingRepository.Edit(productRemainming);
                }
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
                var outboundReceipt = _outboundReceiptRepository.Get((int)request.Id);
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
                    CreatedOn = m.CreatedOn,
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

        public AppResponse<OutboundReceiptDto> GetOutboundReceipt(int Id)
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
                data.CreatedOn = outboundReceipt.CreatedOn;

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
				var product = _outboundReceiptRepository.FindByPredicate(query).OrderByDescending(x => x.CreatedOn);
				int pageIndex = request.PageIndex ?? 1;
				int pageSize = request.PageSize ?? 1;
				int startIndex = (pageIndex - 1) * (int)pageSize;
				var ProductList = product.Skip(startIndex).Take(pageSize)
					.Select(x => new OutboundReceiptDto
					{
						Id = x.Id,
                        To = x.To,
                        WarehouseId = x.WarehouseId,
                        WarehouseName = x.Warehouse.Name,
                        CreatedOn = x.CreatedOn,
					})
					.ToList();


				var searchUserResult = new SearchResponse<OutboundReceiptDto>
				{
					TotalRows = numOfRecords,
					TotalPages = CalculateNumOfPages(numOfRecords, pageSize),
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
                if (Filters!=null)
				foreach (var filter in Filters)
				{
					switch (filter.FieldName)
					{
						case "WarehouseName":
							predicate = predicate.And(m => m.Warehouse.Name.Contains(filter.Value));
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
                                    var day = DateTime.ParseExact(filter.Value, "dd/MM/yyyy", System.Globalization.CultureInfo.InvariantCulture);
                                    //if (filter.Value!="")
                                    predicate = predicate.And(m => m.CreatedOn.Value.Month.Equals(day.Month) && m.CreatedOn.Value.Year.Equals(day.Year));
                                }
                                break;
                            case "Day":
                                {
                                    var day = DateTime.ParseExact(filter.Value, "dd/MM/yyyy", System.Globalization.CultureInfo.InvariantCulture);
                                    //if (filter.Value != "")
                                    predicate = predicate.And(m => m.CreatedOn.Value.Day.Equals(day.Day) && m.CreatedOn.Value.Month.Equals(day.Month) && m.CreatedOn.Value.Year.Equals(day.Year));
                                }
                                break;
                            case "Year":
                                {
                                    var day = DateTime.ParseExact(filter.Value, "dd/MM/yyyy", System.Globalization.CultureInfo.InvariantCulture);
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
