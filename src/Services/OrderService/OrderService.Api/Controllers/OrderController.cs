using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using OrderService.Application.Features.Commands.CreateOrder;
using OrderService.Application.Features.Queries.GetOrderDetailById;
using OrderService.Domain.Models;

namespace OrderService.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderController : ControllerBase
    {
        private readonly IMediator mediator;

        public OrderController(IMediator mediator)
        {
            this.mediator = mediator;
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetOrderDetailsById(Guid id)
        {
            var result = await mediator.Send(new GetOrderDetailsQuery(id));

            return Ok(result);
        }

        [HttpGet]
        [Route("test")]
        public async Task<IActionResult> CreateOrderTest()
        {
            try
            {


                var orderCommand = new CreateOrderCommand(new List<BasketItem>(), "srhtyt", "srhtyt", "Istanbul", "State", "street", "country", "123", "1111111", "srhtyta", DateTime.Now.AddYears(1), "123", 1);
                var result = await mediator.Send(orderCommand);
            }
            catch (Exception ex)
            {
                throw;
            }

            return Ok();
        }
    }
}
