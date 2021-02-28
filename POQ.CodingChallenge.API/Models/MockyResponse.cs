// <copyright file="MockyResponse.cs" company="Bruno DUVAL">
// Copyright (c) Bruno DUVAL.</copyright>

// ReSharper disable UnusedMember.Global
// ReSharper disable InconsistentNaming

using System.Collections.Generic;
using Swashbuckle.AspNetCore.Annotations;

namespace POQ.CodingChallenge.API.Models
{
    public class MockyResponse
    {
        [SwaggerSchema("The product list", ReadOnly = true)]
        public IList<MockyProduct> products { get; set; }

        [SwaggerSchema("The mocky API keys", ReadOnly = true)]
        public MockyApiKeys apiKeys { get; set; }
    }
}