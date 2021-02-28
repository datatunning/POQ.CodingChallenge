// <copyright file="ProductEndpoint.cs" company="Bruno DUVAL">
// Copyright (c) Bruno DUVAL.</copyright>

using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Net.Mime;
using System.Threading;
using System.Threading.Tasks;
using Ardalis.ApiEndpoints;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using POQ.CodingChallenge.API.Services;
using Swashbuckle.AspNetCore.Annotations;

namespace POQ.CodingChallenge.API.Endpoints
{
    [Produces(MediaTypeNames.Application.Json)]
    public class
        ProductEndpoint : BaseAsyncEndpoint.WithRequest<ProductFilterRequest>.WithResponse<IList<ProductFilterResponse>>
    {
        private readonly IProductService _productService;

        public ProductEndpoint(IProductService productService)
        {
            Console.WriteLine(
                $"Initializing Product endpoint {(productService == null ? "without" : "with")} ProductService");
            _productService = productService;
        }

        [HttpGet("/filter")]
        [SwaggerOperation(
            Summary = "List/Filter the products",
            Description = "List/Filter the products",
            OperationId = "Product.Filter",
            Tags = new[] {nameof(ProductEndpoint)})
        ]
        public override async Task<ActionResult<IList<ProductFilterResponse>>> HandleAsync(
            [FromQuery] [SwaggerParameter("The product filtering options")]ProductFilterRequest request,
            CancellationToken cancellationToken = default)
        {
            try
            {
                var response = await _productService.FilterAsync(request, cancellationToken);
                return Ok(response);
            }
            catch (HttpRequestException httpEx)
            {
                if (httpEx.StatusCode == HttpStatusCode.NoContent) return NoContent();

                return Problem(httpEx.Message, null, (int?) httpEx.StatusCode, "Mocky API error");
            }
            catch (Exception ex)
            {
                return Problem(ex.Message, null, (int) HttpStatusCode.InternalServerError, "API error");
            }
        }
    }
}