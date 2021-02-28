// <copyright file="MockyProduct.cs" company="Bruno DUVAL">
// Copyright (c) Bruno DUVAL.</copyright>

// ReSharper disable UnusedMember.Global
// ReSharper disable InconsistentNaming

using Swashbuckle.AspNetCore.Annotations;

namespace POQ.CodingChallenge.API.Models
{
    public class MockyProduct
    {
        [SwaggerSchema("The product title", ReadOnly = true)]
        public string title { get; set; }

        [SwaggerSchema("The product price", ReadOnly = true)]
        public int price { get; set; }

        [SwaggerSchema("The list of available size for this product", ReadOnly = true)]
        public string[] sizes { get; set; }

        [SwaggerSchema("The product description", ReadOnly = true)]
        public string description { get; set; }
    }
}