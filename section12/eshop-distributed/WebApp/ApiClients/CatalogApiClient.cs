using Catalog.Models;

namespace WebApp.ApiClients;

public class CatalogApiClient(HttpClient httpClient)
{
    public async Task<List<Product>> GetProducts()
    {
        var response = await httpClient.GetFromJsonAsync<List<Product>>($"/products");
        return response!;
    }

    public async Task<Product> GetProductById(int id)
    {
        var response = await httpClient.GetFromJsonAsync<Product>($"/products/{id}");
        return response!;
    }
}
