using System.Net.NetworkInformation;
using AutoMapper;
using MayNghien.Common.Helpers;
using MayNghien.Models.Response.Base;
using Microsoft.AspNetCore.Http;
using WarehouseManagement.DAL.Contract;
using WarehouseManagement.DAL.Models.Entity;
using WarehouseManagement.Model.Dto;
using WarehouseManagement.Service.Contract;

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
                var request = new Supplier();
                request = _mapper.Map<Supplier>(supplier);
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
    }
}
