using BasketService.Api.Core.Domain.Models;
using EventBus.Base.Events;

namespace BasketService.Api.IntegrationEvents.Events
{
    public class OrderCreatedIntegrationEvent:IntegrationEvent
    {
        /// <summary>
        /// User Id
        /// </summary>
        public string UserId { get; set; }
        /// <summary>
        /// UserName info
        /// </summary>
        public string UserName { get; set; }
        /// <summary>
        /// Order number created
        /// </summary>
        public int OrderNumber { get; set; }
        /// <summary>
        /// City info
        /// </summary>
        public string City { get; set; }
        /// <summary>
        /// Street info
        /// </summary>
        public string Street { get; set; }
        /// <summary>
        /// State info
        /// </summary>
        public string State { get; set; }
        /// <summary>
        /// Country info
        /// </summary>
        public string Country { get; set; }
        /// <summary>
        /// Zip code info
        /// </summary>
        public string ZipCode { get; set; }
        /// <summary>
        /// Card number
        /// </summary>
        public string CardNumber { get; set; }
        /// <summary>
        /// Card holder name
        /// </summary>
        public string CardHolderName { get; set; }
        /// <summary>
        /// Card expiration time
        /// </summary>
        public DateTime CardExpiration { get; set; }
        /// <summary>
        /// Card security number
        /// </summary>
        public string CardSecurityNumber { get; set; }
        /// <summary>
        /// Card type id
        /// </summary>
        public int CardTypeId { get; set; }
        /// <summary>
        /// Buyer info
        /// </summary>
        public string Buyer { get; set; }
        /// <summary>
        /// Basket info class that contains basket items
        /// </summary>
        public CustomerBasket Basket { get; set; }

        public OrderCreatedIntegrationEvent(string userId, string userName, string city, string street, string state, string country, string zipCode, string cardNumber, string cardHolderName, DateTime cardExpiration, string cardSecurityNumber, int cardTypeId, string buyer,CustomerBasket basket)
        {
            UserId = userId;
            UserName = userName;
            City = city;
            Street = street;
            State = state;
            Country = country;
            ZipCode = zipCode;
            CardNumber = cardNumber;
            CardHolderName = cardHolderName;
            CardExpiration = cardExpiration;
            CardSecurityNumber = cardSecurityNumber;
            CardTypeId = cardTypeId;
            Buyer = buyer;
            Basket = basket;
        }
    }
}
