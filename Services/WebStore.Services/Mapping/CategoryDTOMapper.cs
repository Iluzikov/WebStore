using WebStore.Domain.DTO.Products;
using WebStore.Domain.Entities;

namespace WebStore.Services.Mapping
{
    public static class CategoryDTOMapper
    {
        public static CategoryDTO ToDTO(this Category category) => category is null ? null : new CategoryDTO
        {
            Id = category.Id,
            Name = category.Name,
            Order = category.Order,
            ParentId = category.ParentId,
        };

        public static Category FromDTO(this CategoryDTO category) => category is null ? null : new Category
        {
            Id = category.Id,
            Name = category.Name,
            Order = category.Order,
            ParentId = category.ParentId,
        };
    }
}
