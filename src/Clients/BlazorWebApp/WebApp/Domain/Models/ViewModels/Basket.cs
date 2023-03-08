namespace WebApp.Domain.Models.ViewModels
{
    public class Basket
    {
        /// <summary>
        /// Basket items
        /// </summary>
        public List<BasketItem> Items { get; init; } = new List<BasketItem>();
        /// <summary>
        /// Buyer id
        /// </summary>
        public string BuyerId { get; init; }

        public decimal Total()
        {
            return Math.Round(Items.Sum(x => x.UnitPrice * x.Quantity), 2);
        }
    }
}
