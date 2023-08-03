using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MayNghien.Common.Models;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using WarehouseManagement.DAL.Models.Entity;

namespace WarehouseManagement.DAL.Models.Context
{
    public class WarehouseManagementDbContext : BaseContext
    {
        public WarehouseManagementDbContext()
        {

        }
        public WarehouseManagementDbContext(DbContextOptions options) : base(options)
        {

        }

        
        public DbSet<InboundReceipt>? InboundReceipts { get; set; }
        public DbSet<InventoryReport>? InventoryReports { get; set; }
        public DbSet<Lot>? Lots { get; set; }
        public DbSet<OutboundShipment>? OutboundShipments { get; set; }
        public DbSet<Product>? Products { get; set; }
        public DbSet<Supplier>? Suppliers { get; set; }
        public DbSet<Transfer>? Transfers { get; set; }
        public DbSet<Warehouse>? Warehouses { get; set; }


        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                var appSetting = JsonConvert.DeserializeObject<AppSetting>(File.ReadAllText("appsettings.json"));
                optionsBuilder.UseSqlServer(appSetting.ConnectionString);
            }


        }

    }
}
