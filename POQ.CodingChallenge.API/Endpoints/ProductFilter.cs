// <copyright file="ProductFilter.cs" company="Bruno DUVAL">
// Copyright (c) Bruno DUVAL.</copyright>

using Swashbuckle.AspNetCore.Annotations;

namespace POQ.CodingChallenge.API.Endpoints
{
    /// <summary>
    ///   <br />
    /// </summary>
    public class ProductFilter
    {
        [SwaggerSchema("The minimum price across all product", ReadOnly = true)]
        public int MinPrice { get; set; }

        [SwaggerSchema("The maximum price across all products", ReadOnly = true)]
        public int MaxPrice { get; set; }

        [SwaggerSchema("The product sizes list across all products", ReadOnly = true)]
        public string[] Sizes { get; set; }

        [SwaggerSchema("The 10 most used words except the top 5.", ReadOnly = true)]
        public string[] Keywords { get; set; } = new string[10];
    }
}