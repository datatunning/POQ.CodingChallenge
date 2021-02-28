// <copyright file="ProductResponse.cs" company="Bruno DUVAL">
// Copyright (c) Bruno DUVAL.</copyright>

using System.Collections.Generic;
using POQ.CodingChallenge.API.Models;

namespace POQ.CodingChallenge.API.Endpoints
{
    public class ProductFilter
    {
        public int MinPrice { get; set; }

        public int MaxPrice { get; set; }

        public string[] Sizes { get; set; }

        public string[] Keywords { get; set; } = new string[10];
    }

    public class ProductFilterResponse
    {
        public ProductFilterResponse(IEnumerable<MockyProduct> products)
        {
            Products = products;
            Filter = new ProductFilter();
        }
        public ProductFilterResponse(IEnumerable<MockyProduct> products, (int, int) priceRange, string[] sizes, string[] keywords) : this(products)
        {
            Filter.MinPrice = priceRange.Item1;
            Filter.MaxPrice = priceRange.Item2;
            Filter.Sizes = sizes;
            Filter.Keywords = keywords;
        }

        public IEnumerable<MockyProduct> Products { get; set; }

        public ProductFilter Filter { get; set; }
    }
}