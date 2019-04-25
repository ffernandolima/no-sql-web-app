using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Distributed;
using Newtonsoft.Json;
using NoSqlWebApp.Models;
using System;
using System.Linq;

namespace NoSqlWebApp.Controllers
{
	[Route("api/cart")]
	[ApiController]
	public class CartController : ControllerBase
	{
		private readonly IDistributedCache _distributedCache;

		public CartController(IDistributedCache distributedCache)
		{
			this._distributedCache = distributedCache ?? throw new ArgumentNullException(nameof(distributedCache));
		}

		// GET api/cart/1
		[HttpGet("{userId}", Name = "GetProducts")]
		public IActionResult GetProducts(int userId)
		{
			var key = $"Cart-{userId}";

			var cartJson = this._distributedCache.GetString(key);

			var cart = JsonConvert.DeserializeObject<Cart>(cartJson ?? string.Empty);

			return Ok(cart?.Products ?? Enumerable.Empty<Product>());
		}

		// POST api/cart/1
		[HttpPost("{userId}", Name = "CartCreation")]
		public void CartCreation(long userId)
		{
			var key = $"Cart-{userId}";

			var cart = new Cart { UserId = userId };

			this._distributedCache.SetString(key, JsonConvert.SerializeObject(cart));
		}

		// POST api/cart/1/products
		[HttpPost("{userId}/products", Name = "ProductCreation")]
		public void ProductCreation(long userId, [FromBody]Product product)
		{
			var key = $"Cart-{userId}";

			var cartJson = this._distributedCache.GetString(key);

			var cart = JsonConvert.DeserializeObject<Cart>(cartJson ?? string.Empty);

			if (cart != null)
			{
				cart.Products.Add(product);

				this._distributedCache.SetString(key, JsonConvert.SerializeObject(cart));
			}
		}

		// DELETE api/cart/1/products/2
		[HttpDelete("{userId}/products/{id}")]
		public void ProductRemoval(long userId, long id)
		{
			var key = $"Cart-{userId}";

			var cartJson = this._distributedCache.GetString(key);

			var cart = JsonConvert.DeserializeObject<Cart>(cartJson ?? string.Empty);

			if (cart != null)
			{
				var product = cart.Products.SingleOrDefault(x => x.Id == id);

				if (product != null)
				{
					cart.Products.Remove(product);

					this._distributedCache.SetString(key, JsonConvert.SerializeObject(cart));
				}
			}
		}

		// DELETE api/cart/1
		[HttpDelete("{userId}")]
		public void CartRemoval(long userId)
		{
			var key = $"Cart-{userId}";

			this._distributedCache.Remove(key);
		}
	}
}