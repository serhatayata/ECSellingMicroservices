using Microsoft.AspNetCore.WebUtilities;
using Web.ApiGateway.Extensions;
using Web.ApiGateway.Models.Basket;
using Web.ApiGateway.Services.Interfaces;

namespace Web.ApiGateway.Services
{
    public class BasketService : IBasketService
    {
        private readonly IHttpClientFactory httpClientFactory;

        public BasketService(IHttpClientFactory httpClientFactory)
        {
            this.httpClientFactory = httpClientFactory;
        }

        public async Task<BasketData> GetById(string id)
        {
            var client = httpClientFactory.CreateClient("basket");

            var queryString = new Dictionary<string, string>()
            {
                { "id", id }
            };

            var uri = QueryHelpers.AddQueryString(client.BaseAddress + "/get-basket-by-id", queryString);
            var response = await client.GetResponseAsync<BasketData>(uri);
            return response ?? new BasketData(id);
        }

        public async Task<BasketData> UpdateAsync(BasketData currentBasket)
        {
            var client = httpClientFactory.CreateClient("basket");
            var uri = client.BaseAddress + "/update";
            var response = await client.PostGetResponseAsync<BasketData, BasketData>(uri, currentBasket);
            return response;
        }
    }
}
