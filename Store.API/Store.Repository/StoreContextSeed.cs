using Microsoft.Extensions.Logging;
using Store.Data.Entities;
using Store.Data.Entities.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Store.Repository
{
    public class StoreContextSeed
    {
        public static async Task SeedAsync(StoreDbContext context,ILoggerFactory loggerFactory)
        {
            try
            {
                if (context.ProductBrands != null&&!context.ProductBrands.Any())
                {
                    var productBrand = File.ReadAllText("../Store.Repository/SeedData/brands.json");
                    var brands = JsonSerializer.Deserialize<List<ProductBrand>>(productBrand);

                    if (brands is not null )
                    {
                        await context.ProductBrands.AddRangeAsync(brands);
                        await context.SaveChangesAsync();
                    }
                }

                if (context.ProductTypes != null && !context.ProductTypes.Any())
                {
                    var productTypes = File.ReadAllText("../Store.Repository/SeedData/types.json");
                    var types = JsonSerializer.Deserialize<List<ProductType>>(productTypes);

                    if (types is not null )
                    {
                        context.ProductTypes.AddRange(types);
                        context.SaveChangesAsync();
                    }
                }

                if (context.Products != null && !context.Products.Any())
                {
                    var productsData = File.ReadAllText("../Store.Repository/SeedData/products.json");
                    var product = JsonSerializer.Deserialize<List<Product>>(productsData);

                    if (product is not null)
                    {
                        await context.Products.AddRangeAsync(product);
                        await context.SaveChangesAsync();
                    }
                }
                
                if (context.DeliveryMethods != null && !context.DeliveryMethods.Any())
                {
                    var deliveryMethodsData = File.ReadAllText("../Store.Repository/SeedData/delivery.json");
                    var deliveryMethods = JsonSerializer.Deserialize<List<DeliveryMethod>>(deliveryMethodsData);

                    if (deliveryMethods is not null)
                        await context.DeliveryMethods.AddRangeAsync(deliveryMethods);

                }
                await context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                var logger=loggerFactory.CreateLogger<StoreContextSeed>();

                logger.LogError(ex.Message);
            }
        }
    }
}
