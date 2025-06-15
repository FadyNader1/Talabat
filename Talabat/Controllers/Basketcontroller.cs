using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Talabat.Core.Entities.Basket;
using Talabat.Core.Interfaces;
using Talabat.DTO;
using Talabat.Errors;

namespace Talabat.Controllers
{
    /// <summary>
    /// Controller for managing customer baskets, including creating, updating, retrieving, and deleting baskets.
    /// Provides API endpoints for basket-related operations in the system.
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class Basketcontroller : ControllerBase
    {
        private readonly IBasketRepository basketRepository;

        /// <summary>
        /// Initializes a new instance of the <see cref="Basketcontroller"/> class.
        /// </summary>
        /// <param name="basketRepository">Repository for basket operations.</param>
        public Basketcontroller(IBasketRepository basketRepository)
        {
            this.basketRepository = basketRepository;
        }

        /// <summary>
        /// Creates a new basket or updates an existing basket for a customer.
        /// </summary>
        /// <param name="customerBasket">The basket data to create or update.</param>
        /// <returns>The created or updated <see cref="CustomerBasket"/>.</returns>
        [HttpPost("CreateOrUpdateBasket")]
        public async Task<ActionResult<CustomerBasketDTO>> CreateOrUpdateBasket(CustomerBasketDTO customerBasket)
        {
            if (!ModelState.IsValid)
                return BadRequest(new ApiHandleError(400, "Invalid basket data"));
            var basketmapp=new CustomerBasket
            {
                Id = customerBasket.Id,
                DeliveryMethodId = customerBasket.DeliveryMethodId,
               
                ItemBasket = customerBasket.ItemBasket.Select(item => new ItemBasket
                {
                    Product_Id = item.Product_Id,
                    Quantity = item.Quantity,
                    Price = item.Price
                }).ToList()
            };
            var basket = await basketRepository.CreateOrUpdateAsyc(basketmapp);
            if (basket is null)
                return BadRequest(new ApiHandleError(400, "Basket creation or update failed"));
            return Ok(customerBasket);
        }

        /// <summary>
        /// Retrieves a specific basket by its identifier.
        /// </summary>
        /// <param name="id">The basket identifier.</param>
        /// <returns>The <see cref="CustomerBasket"/> if found.</returns>
        [HttpGet("GetBasketById")]
        public async Task<ActionResult<CustomerBasket>> GetBasketById(string id)
        {
            var basket = await basketRepository.GetBasketByIdAsync(id);
            if (basket is null)
                return NotFound(new ApiHandleError(404, "Basket not found"));
            return Ok(basket);
        }

        /// <summary>
        /// Deletes a basket by its identifier.
        /// </summary>
        /// <param name="id">The identifier of the basket to delete.</param>
        /// <returns>True if the basket was deleted; otherwise, false.</returns>
        [HttpDelete("DeleteBasket")]
        public async Task<ActionResult<bool>> DeleteBasket(string id)
        {
            var result = await basketRepository.DeleteBasketAsync(id);
            if (!result)
                return NotFound(new ApiHandleError(404, "Basket not found or deletion failed"));
            return Ok(result);
        }

        /// <summary>
        /// Retrieves all customer baskets in the system.
        /// </summary>
        /// <returns>A list of all <see cref="CustomerBasket"/>.</returns>
        [HttpGet("GetAllBaskets")]
        public async Task<ActionResult<IReadOnlyList<CustomerBasket>>> GetAllBaskets()
        {
            var baskets = await basketRepository.GetAllBasketAsync();
            if (baskets is null || !baskets.Any())
                return NotFound(new ApiHandleError(404, "No baskets found"));
            return Ok(baskets);
        }

        /// <summary>
        /// Clears all baskets from the system.
        /// </summary>
        /// <returns>True if all baskets were cleared; otherwise, false.</returns>
        [HttpDelete("ClearBasket")]
        public async Task<ActionResult<bool>> ClearBasket()
        {
            var result = await basketRepository.ClearBasketAsync();
            if (!result)
                return NotFound(new ApiHandleError(404, "No baskets found to clear"));
            return Ok(result);
        }
    }
}