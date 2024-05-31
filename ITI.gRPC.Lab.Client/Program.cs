using Grpc.Net.Client;
using ITI.gRPC.lab.client.Protos;
using System;
using System.Threading.Tasks;

var channel = GrpcChannel.ForAddress("https://localhost:7147");
var client = new Inventory.InventoryClient(channel);

var productId = "123";
var productRequest = new ProductRequest { ProductId = productId };
var productResponse = await client.GetProductByIdAsync(productRequest);

if (productResponse.Exists)
{
    // Update product
    var updatedProduct = new Product
    {
        ProductId = productId,
        ProductName = "Updated Product Name",
        ProductDescription = "Updated Product Description",
        ProductPrice = 99.99,
        ProductQuantity = 10
    };
    var updateResponse = await client.UpdateProductAsync(updatedProduct);
    Console.WriteLine($"Product Updated: {updateResponse.Product.ProductName}");
}
else
{
    // Add new product
    var newProduct = new Product
    {
        ProductId = productId,
        ProductName = "New Product",
        ProductDescription = "New Product Description",
        ProductPrice = 49.99,
        ProductQuantity = 5
    };
    var addResponse = await client.AddProductAsync(newProduct);
    Console.WriteLine($"Product Added: {addResponse.Product.ProductName}");
}
