using E_Commerce.Common;
using E_Commerce.Logic;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace E_Commerce.Controllers
{
    [Authorize(Policy = "CustomersOnly")]
    [Route("api/[controller]")]
    [ApiController]
    public class OrderController : ControllerBase
    {
        private readonly IOrderManager _orderManager;
        private readonly ICartManager _cartManager;
        public OrderController(IOrderManager orderManager, ICartManager cartManager)
        {
            _orderManager = orderManager;
            _cartManager = cartManager;
        }
        [HttpPost]
        public async Task<ActionResult<GeneralResult>> PlaceOrder()
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (userId == null)
            {
                var failResult = new GeneralResult { Success = false, Message = "User not authenticated" };
                return Unauthorized(failResult);
            }
            var cart = await _cartManager.GetCartByUserIdAsync(userId);
            if (cart == null)
            {
                var failResult = GeneralResult.FailResult("User dont has cart");
                return BadRequest(failResult);
            }

            var orderRequest = new OrderRequest
            {
                UserId = userId,
                CartId = cart.Data.CartId
            };

            var result = await _orderManager.PlaceOrderAsync(orderRequest);
            if (!result.Success)
            {
                return BadRequest(result);
            }
            return Ok(result);
        }
        [HttpGet]
        public async Task<ActionResult<GeneralResult>> GetOrderHistory()
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (userId == null)
            {
                var failResult = new GeneralResult { Success = false, Message = "User not authenticated" };
                return Unauthorized(failResult);
            }

            var result = await _orderManager.GetOrderHistoryAsync(userId);
            if (!result.Success)
            {
                return BadRequest(result);
            }
            return Ok(result);
        }
        [HttpGet("{id}")]
        public async Task<ActionResult<GenericGeneralResult<OrderDetailsDTO>>> GetOrderDetails(int id)
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (userId == null)
            {
                return Unauthorized(GenericGeneralResult<OrderDetailsDTO>.NotFound("User not authenticated"));
            }

            var result = await _orderManager.GetOrderDetailsAsync(id, userId);

            if (!result.Success)
            {
                return NotFound(result);
            }

            return Ok(result);
        }
    }
}
