namespace BasketService.Api.Core.Domain.Models
{
    public class BasketCheckout
    {
        /// <summary>
        /// City information for order
        /// </summary>
        public string City { get; set; }
        /// <summary>
        /// Street information for order
        /// </summary>
        public string Street { get; set; }
        /// <summary>
        /// State information for order
        /// </summary>
        public string State { get; set; }
        /// <summary>
        /// Country information for order
        /// </summary>
        public string Country { get; set; }
        /// <summary>
        /// ZipCode information for order
        /// </summary>
        public string ZipCode { get; set; }
        /// <summary>
        /// CardNumber information for order
        /// </summary>
        public string CardNumber { get; set; }
        /// <summary>
        /// CardHolderName information for order
        /// </summary>
        public string CardHolderName { get; set; }
        /// <summary>
        /// CardExpiration information for order
        /// </summary>
        public DateTime CardExpiration { get; set; }
        /// <summary>
        /// CardSecurityNumber information for order
        /// </summary>
        public string CardSecurityNumber { get; set; }
        /// <summary>
        /// Card type Id information
        /// </summary>
        public int CardTypeId { get; set; }
        /// <summary>
        /// Buyer Id to get info about which basket's checkout model
        /// </summary>
        public string Buyer { get; set; }
    }
}
