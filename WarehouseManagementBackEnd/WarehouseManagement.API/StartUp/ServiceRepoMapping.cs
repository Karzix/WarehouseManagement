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
            #endregion Service Mapping

            #region Repository Mapping
            builder.Services.AddScoped<WarehouseService, WarehouseService>();

            #endregion Repository Mapping
        }
    }
}
