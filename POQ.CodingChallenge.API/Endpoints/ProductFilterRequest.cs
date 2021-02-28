// <copyright file="ProductFilterRequest.cs" company="Bruno DUVAL">
// Copyright (c) Bruno DUVAL.</copyright>

// ReSharper disable UnusedMember.Global
// ReSharper disable InconsistentNaming
namespace POQ.CodingChallenge.API.Endpoints
{
    /// <summary>
    /// Model used by the endpoint to fill the query parameters.
    /// </summary>
    public class ProductFilterRequest
    {
        public int? maxprice { get; set; }

        public string size { get; set; }

        public string hightlight { get; set; }    }
}