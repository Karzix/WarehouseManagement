using WarehouseManagement.DAL.Contract;
using WarehouseManagement.DAL.Implementation;
using WarehouseManagement.Service.Contract;
using WarehouseManagement.Service.Implementation;

namespace WarehouseManagement.API.StartUp
{
    public class ServiceRepoMapping
    {
        public ServiceRepoMapping() { }

        public void Mapping(WebApplicationBuilder builder)
        {
            #region Service Mapping
            builder.Services.AddScoped<IAuthService, AuthService>();
            builder.Services.AddScoped<IWarehouseService, WarehouseService>();
            builder.Services.AddScoped<IUserManagementService, UserManagamentService>();
            builder.Services.AddScoped<ISupplierService, SupplierService>();
            builder.Services.AddScoped<IProductService, ProductService>();
            builder.Services.AddScoped<ISupplierProductService, SupplierProductService>();
            builder.Services.AddScoped<IOutboundReceiptService, OutboundReceiptService>();
            builder.Services.AddScoped<IExportProductService, ExportProductService>();
            builder.Services.AddScoped<IProductRemainingService, ProductRemainingService>();
            builder.Services.AddScoped<IInboundReceiptService, InboundReceiptService>();
            builder.Services.AddScoped<IImportProductService, ImportProductService>();
            #endregion Service Mapping

            #region Repository Mapping
            builder.Services.AddScoped<IWarehouseRepository, WarehouseRepository>();
            builder.Services.AddScoped<IUserManagementRepository, UserManagementRepository>();
            builder.Services.AddScoped<ISupplierRepository, SupplierRepository>();
            builder.Services.AddScoped<IProductRepository,ProductRepository>();
            builder.Services.AddScoped<ISupplierProductRepository,SupplierProductRepositor>();
            builder.Services.AddScoped<IOutboundReceiptRepository, OutboundReceiptRepository>();
            builder.Services.AddScoped<IExportProductRepository, ExportProductRepository>();
            builder.Services.AddScoped<IProductRemainingRepository, ProductRemainingRepository>();
            builder.Services.AddScoped<IInboundReceiptRepository, InboundReceiptRepository>();
            builder.Services.AddScoped<IImportProductRepository, ImportProductRepository>();
            #endregion Repository Mapping
        }
    }
}
