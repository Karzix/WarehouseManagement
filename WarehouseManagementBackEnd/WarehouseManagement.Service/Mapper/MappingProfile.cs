using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using WarehouseManagement.Model.Dto;
using WarehouseManagement.DAL.Models.Entity;

namespace WarehouseManagement.Service.Mapper
{
    public class MappingProfile:Profile
    {
        public MappingProfile()
        {
            CreateMap();
        }

        public void CreateMap()
        {
            CreateMap<Warehouse, WarehouseDto>().ReverseMap();
        }
    }
}
