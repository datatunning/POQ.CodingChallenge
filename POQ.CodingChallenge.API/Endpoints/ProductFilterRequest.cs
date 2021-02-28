// <copyright file="ProductFilterRequest.cs" company="Bruno DUVAL">
// Copyright (c) Bruno DUVAL.</copyright>

// ReSharper disable UnusedMember.Global
// ReSharper disable InconsistentNaming

using Swashbuckle.AspNetCore.Annotations;

namespace POQ.CodingChallenge.API.Endpoints
{
    /// <summary>
    /// Model used by the endpoint to fill the query parameters.
    /// </summary>
    public class ProductFilterRequest
    {
        [SwaggerSchema("The maximum product size to filter on", ReadOnly = true)]
        public int? maxprice { get; set; }

        [SwaggerSchema("The product size to filter on", ReadOnly = true)]
        public string size { get; set; }

        [SwaggerSchema("The comma separated list of keywords to highlight in the product description", ReadOnly = true)]
        public string hightlight { get; set; }    }
}