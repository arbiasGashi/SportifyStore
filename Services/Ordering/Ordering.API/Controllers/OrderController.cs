using MediatR;
using Microsoft.AspNetCore.Mvc;
using Ordering.Application.Commands;
using Ordering.Application.Queries;
using Ordering.Application.Responses;
using System;
using System.Net;

namespace Ordering.API.Controllers;

public class OrderController : ApiController
{
    private readonly IMediator _mediator;

    public OrderController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet]
    [Route("[action]/{buyer}", Name = "GetOrdersByBuyer")]
    [ProducesResponseType(typeof(IReadOnlyList<OrderSummaryResponse>), (int)HttpStatusCode.OK)]
    public async Task<ActionResult<IReadOnlyList<OrderSummaryResponse>>> GetOrdersByBuyer(string buyer)
    {
        var query = new GetOrdersByBuyerQuery(buyer);
        var result = await _mediator.Send(query);

        return Ok(result);
    }

    [HttpGet]
    [Route("[action]/{orderId:int}", Name = "GetOrderById")]
    [ProducesResponseType(typeof(OrderResponse), (int)HttpStatusCode.OK)]
    public async Task<ActionResult<OrderResponse>> GetOrderById(int orderId)
    {
        var query = new GetOrderByIdQuery(orderId);
        var result = await _mediator.Send(query);

        return Ok(result);
    }

    [HttpPost]
    [Route("[action]", Name = "CheckoutOrder")]
    [ProducesResponseType(typeof(int), (int)HttpStatusCode.OK)]
    public async Task<ActionResult<int>> CheckoutOrder([FromBody] CheckoutOrderCommand command)
    {
        var result = await _mediator.Send(command);

        return Ok(result);
    }

    [HttpPut]
    [Route("[action]", Name = "UpdateOrder")]
    [ProducesResponseType((int)HttpStatusCode.NoContent)]
    public async Task<IActionResult> UpdateOrder([FromBody] UpdateOrderCommand command)
    {
        await _mediator.Send(command);

        return NoContent();
    }

    [HttpDelete]
    [Route("[action]/{orderId:int}", Name = "DeleteOrder")]
    [ProducesResponseType((int)HttpStatusCode.NoContent)]
    public async Task<IActionResult> DeleteOrder(int orderId)
    {
        await _mediator.Send(new DeleteOrderCommand(orderId));

        return NoContent();
    }
}
