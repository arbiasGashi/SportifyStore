using Catalog.Application.Commands;
using Catalog.Application.Queries;
using Catalog.Application.Responses;
using Catalog.Core.Specs;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace Catalog.API.Controllers;

public class CatalogController : ApiController
{
    private readonly IMediator _mediator;

    public CatalogController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet]
    [Route("GetAllProducts")]
    [ProducesResponseType(typeof(Pagination<ProductResponse>), (int)HttpStatusCode.OK)]
    public async Task<ActionResult<IEnumerable<ProductResponse>>> GetAllProducts([FromQuery] CatalogSpecParams catalogSpecParams)
    {
        var query = new GetAllProductsQuery(catalogSpecParams);
        var result = await _mediator.Send(query);

        return Ok(result);
    }

    [HttpGet]
    [Route("[action]/{productName}", Name = "GetProductByName")]
    [ProducesResponseType(typeof(IEnumerable<ProductResponse>), (int)HttpStatusCode.OK)]
    public async Task<ActionResult<IEnumerable<ProductResponse>>> GetProductByName(string productName)
    {
        var query = new GetProductsByNameQuery(productName);
        var result = await _mediator.Send(query);

        return Ok(result);
    }

    [HttpGet]
    [Route("[action]/{id}", Name = "GetProductById")]
    [ProducesResponseType(typeof(ProductResponse), (int)HttpStatusCode.OK)]
    public async Task<ActionResult<ProductResponse>> GetProductById(string id)
    {
        var query = new GetProductByIdQuery(id);
        var result = await _mediator.Send(query);

        return Ok(result);
    }

    [HttpGet]
    [Route("[action]/{brand}", Name = "GetProductsByBrandName")]
    [ProducesResponseType(typeof(IEnumerable<ProductResponse>), (int)HttpStatusCode.OK)]
    public async Task<ActionResult<IEnumerable<ProductResponse>>> GetProductsByBrandName(string brand)
    {
        var query = new GetProductsByBrandQuery(brand);
        var result = await _mediator.Send(query);

        return Ok(result);
    }

    [HttpGet]
    [Route("GetAllBrands")]
    [ProducesResponseType(typeof(IEnumerable<BrandResponse>), (int)HttpStatusCode.OK)]
    public async Task<ActionResult<IEnumerable<BrandResponse>>> GetAllBrands()
    {
        var query = new GetAllBrandsQuery();
        var result = await _mediator.Send(query);

        return Ok(result);
    }

    [HttpGet]
    [Route("GetAllTypes")]
    [ProducesResponseType(typeof(IEnumerable<TypeResponse>), (int)HttpStatusCode.OK)]
    public async Task<ActionResult<IEnumerable<TypeResponse>>> GetAllTypes()
    {
        var query = new GetAllTypesQuery();
        var result = await _mediator.Send(query);

        return Ok(result);
    }

    [HttpPost]
    [Route("CreateProduct")]
    [ProducesResponseType(typeof(ProductResponse), (int)HttpStatusCode.OK)]
    public async Task<ActionResult<ProductResponse>> CreateProduct([FromBody] CreateProductCommand productCommand)
    {
        var result = await _mediator.Send(productCommand);

        return Ok(result);
    }

    [HttpPut]
    [Route("UpdateProduct")]
    [ProducesResponseType(typeof(bool), (int)HttpStatusCode.OK)]
    public async Task<ActionResult<bool>> UpdateProduct([FromBody] UpdateProductCommand productCommand)
    {
        var result = await _mediator.Send(productCommand);

        return Ok(result);
    }

    [HttpDelete]
    [Route("DeleteProduct")]
    [ProducesResponseType(typeof(bool), (int)HttpStatusCode.OK)]
    public async Task<ActionResult<bool>> DeleteProduct(string id)
    {
        var command = new DeleteProductByIdCommand(id);
        var result = await _mediator.Send(command);

        return Ok(result);
    }
}
