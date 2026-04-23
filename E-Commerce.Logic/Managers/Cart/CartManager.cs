using E_Commerce.Common;
using E_Commerce.Data;

namespace E_Commerce.Logic
{
    public class CartManager : ICartManager
    {
        private readonly IUnitOfWork _unitOfWork;
        public CartManager(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public async Task<GenericGeneralResult<CartReadDTO>> GetCartByUserIdAsync(string userId)
        {
            var cart = await _unitOfWork._CartRepository
                                        .GetCartWithItemsByUserIdAsync(userId);

            if (cart is null)
            {
                var createdCart = new Cart
                {
                    UserId = userId,
                };
                _unitOfWork._CartRepository.Add(createdCart);
                await _unitOfWork.SaveAsync();
                var createdCartDTO = new CartReadDTO
                {
                    CartId = createdCart.Id,
                    UserId = createdCart.UserId,
                };
                return GenericGeneralResult<CartReadDTO>.SuccessResult(createdCartDTO);
            }
            var cartItems = await _unitOfWork._CartItemRepository
                                             .GetCartItemsOfCartWithProductAsync(cart.Id);

            var cartItemDTOs = cartItems.Select(ci => new CartItemReadDTO
            {
                Id = ci.Id,
                CartId = ci.CartId,
                ProductId = ci.ProductId,
                ProductName = ci.Product.Name,
                ProductPrice = ci.Product.Price,
                Quantity = ci.Quantity
            }).ToList();

            var cartDTO = new CartReadDTO
            {
                CartId = cart.Id,
                UserId = cart.UserId,
                CartItems = cartItemDTOs
            };

            return GenericGeneralResult<CartReadDTO>.SuccessResult(cartDTO);
        }

        public async Task<GeneralResult> AddToCartAsync(string userId, int productId, int quantity)
        {
            var cart = await _unitOfWork._CartRepository.GetCartWithItemsByUserIdAsync(userId);
            if (cart is null)
            {
                cart = new Cart
                {
                    UserId = userId,
                    CartItems = new List<CartItem>()
                };
                _unitOfWork._CartRepository.Add(cart);
                await _unitOfWork.SaveAsync();
            }

            var product = await _unitOfWork._ProductRepository.GetByIdAsync(productId);
            if (product is null)
            {
                return GeneralResult.NotFound("Product not found.");
            }

            if (product.Count < quantity)
            {
                return GeneralResult.FailResult("Not enough stock for the requested product.");
            }

            product.Count -= quantity;

            var cartItem = new CartItem
            {
                CartId = cart.Id,
                ProductId = productId,
                Quantity = quantity,
            };
            _unitOfWork._CartItemRepository.Add(cartItem);
            await _unitOfWork.SaveAsync();
            return GeneralResult.SuccessResult("Product added to cart successfully.");
        }
        public async Task<GeneralResult> RemoveFromCartAsync(string userId, int cartItemId)
        {
            var cart = await _unitOfWork._CartRepository.GetCartWithItemsByUserIdAsync(userId);

            var cartItem = await _unitOfWork._CartItemRepository.GetByIdAsync(cartItemId);
            if (cartItem is null || cartItem.CartId != cart.Id)
            {
                return GeneralResult.NotFound("Cart item not found.");
            }

            var product = await _unitOfWork._ProductRepository.GetByIdAsync(cartItem.ProductId);
            if (product is not null)
            {
                product.Count += cartItem.Quantity;
            }
            _unitOfWork._CartItemRepository.Delete(cartItem);
            await _unitOfWork.SaveAsync();
            return GeneralResult.SuccessResult("Product removed from cart successfully.");
        }
        public async Task<GeneralResult> UpdateCartItemQuantityAsync(string userId, int cartItemId, int quantity)
        {
            var cart = await _unitOfWork._CartRepository.GetCartWithItemsByUserIdAsync(userId);

            var cartItem = await _unitOfWork._CartItemRepository.GetByIdAsync(cartItemId);
            if (cartItem is null || cartItem.CartId != cart.Id)
            {
                return GeneralResult.NotFound("Cart item not found.");
            }

            var product = await _unitOfWork._ProductRepository.GetByIdAsync(cartItem.ProductId);
            if (product is null)
            {
                return GeneralResult.NotFound("Product not found.");
            }

            int quantityDifference = quantity - cartItem.Quantity;
            if (quantityDifference > 0 && product.Count < quantityDifference)
            {
                return GeneralResult.FailResult("Not enough stock for the requested product.");
            }

            product.Count -= quantityDifference;
            cartItem.Quantity = quantity;
            await _unitOfWork.SaveAsync();
            return GeneralResult.SuccessResult("Cart item quantity updated successfully.");
        }
    }
}
