using WebStore.Domain.DTO.Products;
using WebStore.Domain.Entities;

namespace WebStore.Services.Mapping
{
    public static class BrandDTOMapper
    {
        public static BrandDTO ToDTO(this Brand brand) => brand is null ? null : new BrandDTO
        {
            Id = brand.Id,
            Name = brand.Name,
            Order = brand.Order,
        };

        public static Brand FromDTO(this BrandDTO brand) => brand is null ? null : new Brand
        {
            Id = brand.Id,
            Name = brand.Name,
            Order = brand.Order,
        };
    }
}
