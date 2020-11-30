﻿using System.Collections.Generic;
using System.Linq;
using WebStore.Domain.Entities;
using WebStore.Domain.ViewModels;

namespace WebStore.Services.Mapping
{
    public static class ProductMapper
    {
        public static ProductViewModel ToView(this Product p) => new ProductViewModel
        {
            Id = p.Id,
            Name = p.Name,
            Order = p.Order,
            Price = p.Price,
            ImageUrl = p.ImageUrl,
            BrandName = p.Brand?.Name
        };

        public static IEnumerable<ProductViewModel> ToView(this IEnumerable<Product> products) => products.Select(ToView);
    }
}
