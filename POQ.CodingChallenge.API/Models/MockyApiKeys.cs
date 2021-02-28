// <copyright file="MockyApiKeys.cs" company="Bruno DUVAL">
// Copyright (c) Bruno DUVAL.</copyright>

// ReSharper disable UnusedMember.Global
// ReSharper disable InconsistentNaming

using System.Diagnostics.CodeAnalysis;

namespace POQ.CodingChallenge.API.Models
{
    [ExcludeFromCodeCoverage]
    public class MockyApiKeys
    {
        public string primary { get; set; }
        public string secondary { get; set; }
    }
}