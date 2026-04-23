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
    public class CartController : ControllerBase
    {
        private readonly ICartManager _cartManager;
        public CartController(ICartManager cartManager)
        {
            _cartManager = cartManager;
        }
        [HttpPost]
        [Route("add")]
        public async Task<ActionResult<GeneralResult>> AddToCart([FromBody] AddToCartDTO addToCartDTO)
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (userId == null)
            {
                var failResult = new GeneralResult { Success = false, Message = "User not authenticated" };
                return Unauthorized(failResult);
            }

            var result = await _cartManager.AddToCartAsync(userId, addToCartDTO.ProductId, addToCartDTO.Quantity);
            if (!result.Success)
            {
                return BadRequest(result);
            }
            return Ok(result);
        }
        [HttpDelete]
        [Route("Delete/{cartItemId:int}")]
        public async Task<ActionResult<GeneralResult>> RemoveFromCart(int cartItemId)
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (userId == null)
            {
                var failResult = new GeneralResult { Success = false, Message = "User not authenticated" };
                return Unauthorized(failResult);
            }

            var result = await _cartManager.RemoveFromCartAsync(userId, cartItemId);
            if (!result.Success)
            {
                return BadRequest(result);
            }
            return Ok(result);
        }
        [HttpPut]
        [Route("Update")]
        public async Task<ActionResult<GeneralResult>> UpdateCartItemQuantity ([FromBody] UpdateCartDTO updateCartDTO)
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (userId == null)
            {
                var failResult = new GeneralResult { Success = false, Message = "User not authenticated" };
                return Unauthorized(failResult);
            }

            var result = await _cartManager.UpdateCartItemQuantityAsync(userId, updateCartDTO.CartItemId, updateCartDTO.Quantity);
            if (!result.Success)
            {
                return BadRequest(result);
            }
            return Ok(result);
        }
        [HttpGet]
        public async Task<ActionResult<GenericGeneralResult<CartReadDTO>>> GetCart()
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (userId == null)
            {
                var failResult = new GenericGeneralResult<CartReadDTO> { Success = false, Message = "User not authenticated" };
                return Unauthorized(failResult);
            }

            var result = await _cartManager.GetCartByUserIdAsync(userId);
            if (!result.Success)
            {
                return BadRequest(result);
            }
            return Ok(result);
        }
    }
}
