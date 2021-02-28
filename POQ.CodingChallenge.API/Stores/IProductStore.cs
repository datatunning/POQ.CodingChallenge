// <copyright file="IProductStore.cs" company="Bruno DUVAL">
// Copyright (c) Bruno DUVAL.</copyright>

using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using POQ.CodingChallenge.API.Models;

namespace POQ.CodingChallenge.API.Stores
{
    /// <summary>
    /// In memory storage for data caching.
    /// </summary>
    public interface IProductStore
    {
        /// <summary>Reals all.</summary>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>
        ///   <br />
        /// </returns>
        public Task<IList<MockyProduct>> RealAll(CancellationToken cancellationToken);
    }
}