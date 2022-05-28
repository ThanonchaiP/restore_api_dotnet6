using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using EcomApi.Data;
using EcomApi.Models;
using EcomApi.DTOs;

namespace EcomApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BasketController : ControllerBase
    {
        private readonly APIContext _context;

        public BasketController(APIContext context)
        {
            _context = context;
        }

        // GET: api/Basket
        [HttpGet]
        public async Task<ActionResult<BasketDto>> GetBasket()
        {
            var basket = await RetrieveBasket();

            if (basket == null) return NotFound();
            return MapBasketToDto(basket);
        }

        // POST: api/Basket
        [HttpPost]
        public async Task<ActionResult<BasketDto>> AddItemToBasket(int productId, int quantity)
        {
            var basket = await RetrieveBasket();
            if (basket == null) basket = CreateBasket();

            var product = await _context.Product.FindAsync(productId);
            if (product == null) return NotFound();

            basket.AddItem(product, quantity);
            var result = await _context.SaveChangesAsync() > 0;

            if (result) return CreatedAtAction("GetBasket", MapBasketToDto(basket));

            return BadRequest(new { message = "Problem saving item to basket" });
        }


        [HttpDelete]
        public async Task<IActionResult> RemoveBasketItem(int productId, int quantity)
        {
            var basket = await RetrieveBasket();
            if (basket == null) return NotFound();

            basket.RemoveItem(productId, quantity);
            var result = await _context.SaveChangesAsync() > 0;

            if (result) return NoContent();

            return BadRequest(new ProblemDetails { Title = "Problem removing item from the basket" });
        }

        private async Task<Basket> RetrieveBasket()
        {
            var result = await _context.Basket
                .Include(i => i.Items)
                .ThenInclude(p => p.Product)
                .FirstOrDefaultAsync(x => x.BuyerId == "d83d2676-5017-4efc-a79c-40366afa64bb"); //Request.Cookies["buyerId"]);
            return result!;
        }
        private Basket CreateBasket()
        {
            var buyerId = Guid.NewGuid().ToString();
            var cookieOptions = new CookieOptions { IsEssential = true, Expires = DateTime.Now.AddDays(30) };
            Response.Cookies.Append("buyerId", buyerId, cookieOptions);

            var basket = new Basket { BuyerId = buyerId };
            _context.Basket.Add(basket);
            return basket;
        }

        private BasketDto MapBasketToDto(Basket basket)
        {
            return new BasketDto
            {
                Id = basket.Id,
                BuyerId = basket.BuyerId,
                Items = basket.Items.Select(item => new BasketItemDto
                {
                    ProductId = item.ProductId,
                    Name = item.Product.Name,
                    Price = item.Product.Price,
                    PictureUrl = item.Product.PictureUrl,
                    Type = item.Product.Type,
                    Brand = item.Product.Brand,
                    Quantity = item.Quantity
                }).ToList(),
            };
        }

    }
}
