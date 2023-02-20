namespace BasketService.Api.Core.Domain.Models
{
    public class CustomerBasket
    {
        /// <summary>
        /// Buyer Id if exists
        /// </summary>
        public string BuyerId { get; set; }
        /// <summary>
        /// Basket items
        /// </summary>
        public List<BasketItem> Items { get; set; } = new();

        public CustomerBasket()
        {

        }

        public CustomerBasket(string customerId)
        {
            BuyerId = customerId;
        }
    }
}
