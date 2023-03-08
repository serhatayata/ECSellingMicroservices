using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Web.ApiGateway.Models.Basket;

namespace Web.ApiGateway.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BasketController : ControllerBase
    {
        [HttpPost]
        [Route("items")]
        public async Task<IActionResult> AddBasketItemAsync([FromBody] AddBasketItemRequest request)
        {

        }
    }
}
