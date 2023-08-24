using AutoMapper;
using MayNghien.Models.Response.Base;
using WarehouseManagement.DAL.Contract;
using WarehouseManagement.DAL.Models.Entity;
using WarehouseManagement.Model.Dto;
using WarehouseManagement.Service.Contract;

namespace WarehouseManagement.Service.Implementation
{
    public class WarehouseService : IWarehouseService
    {
        private readonly IWarehouseRepository _warehouseRepository;
        private readonly IMapper _mapper;

        public WarehouseService(IWarehouseRepository warehouseRepository, IMapper mapper)
        {
            _warehouseRepository = warehouseRepository;
            _mapper = mapper;
        }

        public AppResponse<WarehouseDto> CreateWarehouse(WarehouseDto request)
        {
            var result = new AppResponse<WarehouseDto>();
            try
            {
                var warehouse = new Warehouse();
                warehouse = _mapper.Map<Warehouse>(request);
                warehouse.Id = Guid.NewGuid();
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

        public AppResponse<string> DeleteWarehouse(Guid Id)
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

        public AppResponse<WarehouseDto> GetWarehouseById(Guid Id)
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
    }
}
