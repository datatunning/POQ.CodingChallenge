// <copyright file="MockyResponse.cs" company="Bruno DUVAL">
// Copyright (c) Bruno DUVAL.</copyright>

// ReSharper disable UnusedMember.Global
// ReSharper disable InconsistentNaming

using System.Collections.Generic;

namespace POQ.CodingChallenge.API.Models
{
    public class MockyResponse
    {
        public IList<MockyProduct> products { get; set; }
        public MockyApiKeys apiKeys { get; set; }
    }
}