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

        public DbSet<Warehouse> Warehouse { get; set; }
        public DbSet<Product> Product { get; set; }
        public DbSet<Supplier> Supplier { get; set; }
        public DbSet<ProductRemaining> ProductRemaining { get; set; }
        public DbSet<SupplierProduct> SupplierProduct { get; set; }
        //public DbSet<ProductReceipt> ProductReceipt { get; set; }
        //public DbSet<Consignment> Consignment { get; set; }
        public DbSet<ExportProduct> ExportProduct { get; set; }
        public DbSet<ImportProduct> ImportProduct { get; set; }
        public DbSet<InboundReceipt> InboundReceipt { get; set; }
        public DbSet<OutboundReceipt> OutboundReceipt { get; set; }
       


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
