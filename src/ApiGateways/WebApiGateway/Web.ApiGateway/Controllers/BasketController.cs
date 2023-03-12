using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using Web.ApiGateway.Models.Basket;
using Web.ApiGateway.Services.Interfaces;

namespace Web.ApiGateway.Controllers
{
    [EnableCors("CorsPolicy")]
    [Route("[controller]")]
    [ApiController]
    public class BasketController : ControllerBase
    {
        private readonly ICatalogService catalogService;
        private readonly IBasketService basketService;

        public BasketController(ICatalogService catalogService, IBasketService basketService)
        {
            this.catalogService = catalogService;
            this.basketService = basketService;
        }

        [HttpPost]
        [Route("items-add")]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        public async Task<IActionResult> AddBasketItemAsync([FromBody] AddBasketItemRequest request)
        {   
            if (request is null ||request.Quantity == 0)
                return BadRequest("Invalid payload");

            var item = await catalogService.GetCatalogItemAsync(request.CatalogItemId);

            var currentBasket = await basketService.GetById(request.BasketId);

            var productExists = currentBasket.Items.SingleOrDefault(i => i.ProductId == item.Id);
            if (productExists != null)
                productExists.Quantity += request.Quantity;
            else
                currentBasket.Items.Add(new BasketDataItem() { UnitPrice = item.Price, PictureUrl = item.PictureUri ?? string.Empty, 
                                                               ProductId = item.Id, Quantity = request.Quantity, 
                                                               Id = Guid.NewGuid().ToString(), ProductName = item.Name });

            await basketService.UpdateAsync(currentBasket);

            return Ok();
        }
    }
}
