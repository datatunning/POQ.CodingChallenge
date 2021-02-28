// <copyright file="IProductService.cs" company="Bruno DUVAL">
// Copyright (c) Bruno DUVAL.</copyright>

using System.Threading;
using System.Threading.Tasks;
using POQ.CodingChallenge.API.Endpoints;

namespace POQ.CodingChallenge.API.Services
{
    /// <summary>
    ///     Service feature for the Product
    /// </summary>
    public interface IProductService
    {
        /// <summary>Filters the specified request.</summary>
        /// <param name="request">The request.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>
        ///     <br />
        /// </returns>
        public Task<ProductFilterResponse> FilterAsync(ProductFilterRequest request,
            CancellationToken cancellationToken);
    }
}