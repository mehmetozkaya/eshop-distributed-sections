using Microsoft.Extensions.Caching.Distributed;
using System.Text.Json;

namespace Basket.Services;

public class BasketService(IDistributedCache cache, CatalogApiClient catalogApiClient)
{
    public async Task<ShoppingCart?> GetBasket(string userName)
    {
        var basket = await cache.GetStringAsync(userName);
        return string.IsNullOrEmpty(basket) ? null :
            JsonSerializer.Deserialize<ShoppingCart>(basket);
    }
    public async Task UpdateBasket(ShoppingCart basket)
    {
        // Before update(Add/remove Item) into SC, we should call Catalog ms GetProductById method
        // Get latest product information and set Price and ProductName when adding item into SC
        foreach (var item in basket.Items)
        {
            var product = await catalogApiClient.GetProductById(item.ProductId);
            item.Price = product.Price;
            item.ProductName = product.Name;
        }

        await cache.SetStringAsync(basket.UserName, JsonSerializer.Serialize(basket));
    }
    public async Task DeleteBasket(string userName)
    {
        await cache.RemoveAsync(userName);
    }
}
