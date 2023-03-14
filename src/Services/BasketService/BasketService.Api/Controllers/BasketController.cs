using BasketService.Api.Core.Application.Repository;
using BasketService.Api.Core.Application.Services;
using BasketService.Api.Core.Domain.Models;
using BasketService.Api.IntegrationEvents.Events;
using EventBus.Base.Abstraction;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace BasketService.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class BasketController : ControllerBase
    {
        private readonly IBasketRepository _basketRepository;
        private readonly IIdentityService _identityService;
        private readonly IEventBus _eventBus;
        private readonly ILogger<BasketController> _logger;

        public BasketController(IBasketRepository basketRepository, IIdentityService identityService, IEventBus eventBus, ILogger<BasketController> logger)
        {
            _basketRepository = basketRepository;
            _identityService = identityService;
            _eventBus = eventBus;
            _logger = logger;
        }

        #region Get
        [HttpGet]
        public IActionResult Get()
        {
            return Ok("Basket Service is up and running");
        }
        #endregion
        #region GetBasketByIdAsync
        [HttpGet]
        [Route("get-basket-by-id")]
        [ProducesResponseType(typeof(CustomerBasket), (int)HttpStatusCode.OK)]
        public async Task<ActionResult<CustomerBasket>> GetBasketByIdAsync([FromQuery] string id)
        {
            var basket = await _basketRepository.GetBasketAsync(id);
            return Ok(basket ?? new CustomerBasket(id));
        }
        #endregion
        #region UpdateBasketAsync
        [HttpPost]
        [Route("update")]
        [ProducesResponseType(typeof(CustomerBasket), (int)HttpStatusCode.OK)]
        public async Task<ActionResult<CustomerBasket>> UpdateBasketAsync([FromBody] CustomerBasket model)
        {
            var result = await _basketRepository.UpdateBasketAsync(model);
            return Ok(result);
        }
        #endregion
        #region AddItemToBasket
        [HttpPost]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [Route("additem")]
        public async Task<ActionResult> AddItemToBasketAsync([FromBody] BasketItem basketItem)
        {
            var userId = _identityService.GetUserName();

            var basket = await _basketRepository.GetBasketAsync(userId);
            if (basket == null)
                basket = new CustomerBasket(userId);

            basket.Items.Add(basketItem);
            await _basketRepository.UpdateBasketAsync(basket);

            return Ok();
        }
        #endregion
        #region CheckoutAsync
        [HttpPost]
        [Route("checkout")]
        [ProducesResponseType((int)HttpStatusCode.Accepted)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        public async Task<ActionResult> CheckoutAsync([FromBody] BasketCheckout basketCheckout /*[FromHeader(Name = "x-request-id")] string requestId*/)
        {
            var userId = basketCheckout.Buyer;

            var basket = await _basketRepository.GetBasketAsync(userId);
            if (basket == null)
                return BadRequest();

            var userName = _identityService.GetUserName();

            var eventMessage = new OrderCreatedIntegrationEvent(userId, userName, basketCheckout.City,basketCheckout.Street,
                                                                                  basketCheckout.State,basketCheckout.Country,
                                                                                  basketCheckout.ZipCode,basketCheckout.CardNumber,
                                                                                  basketCheckout.CardHolderName,basketCheckout.CardExpiration,
                                                                                  basketCheckout.CardSecurityNumber,basketCheckout.CardTypeId,
                                                                                  basketCheckout.Buyer,
                                                                                  basket);
            try
            {
                // Order api publish to start process
                _eventBus.Publish(eventMessage);
                // Here, after publishing the event, we might delete the basket but it is not a correct way.
                // We should be listening the same order created event that was published here, and when the event arrives rabbitmq, then we clear the basket.
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error publishing integration event : {IntegrationEventId} from {BasketService.App}", eventMessage.Id);
                throw;
            }

            return Accepted();
        }
        #endregion
        #region DeleteAsync
        [HttpDelete]
        [Route("delete")]
        [ProducesResponseType(typeof(void),(int)HttpStatusCode.OK)]
        public async Task DeleteBasketByIdAsync([FromQuery] string id)
        {
            await _basketRepository.DeleteBasketAsync(id);
        }
        #endregion
    }
}
