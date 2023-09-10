using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using WarehouseManagement.Model.Dto;
using WarehouseManagement.DAL.Models.Entity;
using Microsoft.AspNetCore.Identity;

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
            CreateMap<IdentityUser, UserModel>().ReverseMap();
            CreateMap<Supplier, SupplierDto>().ReverseMap();
            CreateMap<Product, ProductDto>().ReverseMap();
            CreateMap<SupplierProduct, SupplierProductDto>().ReverseMap();
            CreateMap<OutboundReceipt, OutboundReceiptDto>().ReverseMap();
        }
    }
}
