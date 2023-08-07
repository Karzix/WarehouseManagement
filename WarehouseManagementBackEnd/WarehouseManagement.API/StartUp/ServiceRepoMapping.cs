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

            #endregion Service Mapping

            #region Repository Mapping
            

            #endregion Repository Mapping
        }
    }
}
