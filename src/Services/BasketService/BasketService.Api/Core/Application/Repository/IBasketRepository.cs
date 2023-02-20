using BasketService.Api.Core.Domain.Models;

namespace BasketService.Api.Core.Application.Repository
{
    public interface IBasketRepository
    {
        /// <summary>
        /// Get basket by consumer Id
        /// </summary>
        /// <param name="consumerId">consumer Id</param>
        /// <returns></returns>
        Task<CustomerBasket> GetBasketAsync(string customerId);
        /// <summary>
        /// Get all users
        /// </summary>
        /// <returns>List of users</returns>
        IEnumerable<string> GetUsers();
        /// <summary>
        /// Update basket items
        /// </summary>
        /// <param name="basket">basket model</param>
        /// <returns></returns>
        Task<CustomerBasket> UpdateBasketAsync(CustomerBasket basket);
        /// <summary>
        /// Delete basket
        /// </summary>
        /// <param name="id">id</param>
        /// <returns></returns>
        Task<bool> DeleteBasketAsync(string id);
    }
}
