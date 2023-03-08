namespace WebApp.Domain.Models.ViewModels
{
    public class BasketItem
    {
        /// <summary>
        /// Id
        /// </summary>
        public string Id { get; set; }
        /// <summary>
        /// Product id
        /// </summary>
        public int ProductId { get; set; }
        /// <summary>
        /// Product name
        /// </summary>
        public string ProductName { get; set; }
        /// <summary>
        /// Unit price
        /// </summary>
        public decimal UnitPrice { get; set; }
        /// <summary>
        /// Quantity of product
        /// </summary>
        public int Quantity { get; set; }
        /// <summary>
        /// Picture url
        /// </summary>
        public string PictureUrl { get; set; }
    }
}
