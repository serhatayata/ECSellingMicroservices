using System.ComponentModel.DataAnnotations;

namespace BasketService.Api.Core.Domain.Models
{
    public class BasketItem : IValidatableObject
    {
        /// <summary>
        /// Basket item id
        /// </summary>
        public string Id { get; set; }
        /// <summary>
        /// Basket product id
        /// </summary>
        public int ProductId { get; set; }
        /// <summary>
        /// Basket product name
        /// </summary>
        public string ProductName { get; set; } = string.Empty;
        /// <summary>
        /// Basket product unit price - without discount
        /// </summary>
        public decimal UnitPrice { get; set; }
        /// <summary>
        /// Basket product old unit price - with discount
        /// </summary>
        public decimal OldUnitPrice { get; set; }
        /// <summary>
        /// Basket product's quantity
        /// </summary>
        public int Quantity { get; set; }
        /// <summary>
        /// Basket product's picture url
        /// </summary>
        public string PictureUrl { get; set; } = string.Empty;

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            var results = new List<ValidationResult>();

            if (Quantity < 1)
                results.Add(new ValidationResult("Invalid number of units", new[] { "Quantity" }));

            return results;
        }
    }
}
