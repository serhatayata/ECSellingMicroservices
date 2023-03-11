namespace Web.ApiGateway.Models.Basket
{
    public class BasketDataItem
    {
        /// <summary>
        /// Id of basket
        /// </summary>
        public string Id { get; set; }
        /// <summary>
        /// Id of product
        /// </summary>
        public int ProductId { get; set; }
        /// <summary>
        /// Product name
        /// </summary>
        public string ProductName { get; set; } = string.Empty;
        /// <summary>
        /// Selling price for product
        /// </summary>
        public decimal UnitPrice { get; set; }
        /// <summary>
        /// Quantity of the product
        /// </summary>
        public int Quantity { get; set; }
        /// <summary>
        /// Picture url of the product
        /// </summary>
        public string PictureUrl { get; set; } = string.Empty;
    }
}
