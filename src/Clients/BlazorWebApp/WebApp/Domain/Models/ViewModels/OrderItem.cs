namespace WebApp.Domain.Models.ViewModels
{
    public class OrderItem
    {
        /// <summary>
        /// Id of product
        /// </summary>
        public int ProductId { get; set; }
        /// <summary>
        /// Name of product
        /// </summary>
        public string ProductName { get; set; }
        /// <summary>
        /// Unit price
        /// </summary>
        public decimal UnitPrice { get; set; }
        /// <summary>
        /// Discount rate
        /// </summary>
        public decimal Discount { get; set; }
        /// <summary>
        /// Units
        /// </summary>
        public int Units { get; set; }
        /// <summary>
        /// Picture url
        /// </summary>
        public string PictureUrl { get; set; }
    }
}
