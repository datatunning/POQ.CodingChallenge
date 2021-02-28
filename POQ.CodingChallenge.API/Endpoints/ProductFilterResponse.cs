// <copyright file="ProductResponse.cs" company="Bruno DUVAL">
// Copyright (c) Bruno DUVAL.</copyright>

using System.Collections.Generic;
using POQ.CodingChallenge.API.Models;
using Swashbuckle.AspNetCore.Annotations;

namespace POQ.CodingChallenge.API.Endpoints
{
    public class ProductFilterResponse
    {
        /// <summary>Initializes a new instance of the <see cref="ProductFilterResponse" /> class.</summary>
        /// <param name="products">The products.</param>
        public ProductFilterResponse(IEnumerable<MockyProduct> products)
        {
            Products = products;
            Filter = new ProductFilter();
        }

        /// <summary>Initializes a new instance of the <see cref="ProductFilterResponse" /> class.</summary>
        /// <param name="products">The products.</param>
        /// <param name="priceRange">The price range.</param>
        /// <param name="sizes">The sizes.</param>
        /// <param name="keywords">The keywords.</param>
        public ProductFilterResponse(IEnumerable<MockyProduct> products, (int min, int max) priceRange, string[] sizes, string[] keywords) : this(products)
        {
            Filter.MinPrice = priceRange.min;
            Filter.MaxPrice = priceRange.max;
            Filter.Sizes = sizes;
            Filter.Keywords = keywords;
        }

        [SwaggerSchema("The product list", ReadOnly = true)]
        public IEnumerable<MockyProduct> Products { get; set; }

        [SwaggerSchema("The options available to filter the products", ReadOnly = true)]
        public ProductFilter Filter { get; set; }
    }
}