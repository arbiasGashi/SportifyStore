using Basket.Application.Commands;
using Basket.Application.Queries;
using Basket.Application.Responses;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace Basket.API.Controllers;

public class BasketController : ApiController
{
    private readonly IMediator _mediator;

    public BasketController(IMediator mediator)
    {
        _mediator = mediator;
    }

    // GET api/v1/Basket/{userName}
    [HttpGet("{userName}", Name = "GetBasketByUserName")]
    [ProducesResponseType(typeof(ShoppingCartResponse), (int)HttpStatusCode.OK)]
    public async Task<ActionResult<ShoppingCartResponse>> GetBasket(string userName)
    {
        var query = new GetBasketByUserNameQuery(userName);
        var basket = await _mediator.Send(query);

        return Ok(basket);
    }

    // POST api/v1/Basket
    // Upsert (Create or Update) shopping cart
    [HttpPost(Name = "UpsertBasket")]
    [ProducesResponseType(typeof(ShoppingCartResponse), (int)HttpStatusCode.OK)]
    public async Task<ActionResult<ShoppingCartResponse>> UpsertBasket([FromBody] UpsertShoppingCartCommand command)
    {
        var result = await _mediator.Send(command);

        return Ok(result);
    }

    // DELETE api/v1/Basket/{userName}
    [HttpDelete("{userName}", Name = "DeleteBasketByUserName")]
    [ProducesResponseType((int)HttpStatusCode.NoContent)]
    public async Task<IActionResult> DeleteBasket(string userName)
    {
        await _mediator.Send(new DeleteBasketByUserNameCommand(userName));

        return NoContent();
    }
}
