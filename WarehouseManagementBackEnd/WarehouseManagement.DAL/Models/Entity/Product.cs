using MayNghien.Common.Models.Entity;

namespace WarehouseManagement.DAL.Models.Entity
{
    public class Product:BaseEntity
    {
        public string? Name { get; set; }
        public string? Description { get; set; }
        public int? Quantity { get; set; }
    }
}
