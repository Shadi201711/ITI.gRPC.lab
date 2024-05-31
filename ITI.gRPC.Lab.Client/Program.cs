using Grpc.Net.Client;
using ITI.gRPC.lab.client.Protos;
using System;
using System.Threading.Tasks;

var channel = GrpcChannel.ForAddress("https://localhost:7147");
var client = new Inventory.InventoryClient(channel);

Console.WriteLine("Client: Connected to server.");

// Default product ID to search and update
var defaultProductId = "123";

var productRequest = new ProductRequest { ProductId = defaultProductId };
Console.WriteLine($"Client: Sending request to get product with ID: {defaultProductId}");
var productResponse = await client.GetProductByIdAsync(productRequest);
Console.WriteLine($"Client: Received response. Product exists: {productResponse.Exists}");

if (productResponse.Exists)
{
    
    Console.WriteLine($"Client: Product with ID {defaultProductId} exists. Please provide updated product details:");

    Console.WriteLine("Enter product name:");
    var updatedProductName = Console.ReadLine();

    Console.WriteLine("Enter product description:");
    var updatedProductDescription = Console.ReadLine();

    Console.WriteLine("Enter product price:");
    var updatedProductPrice = double.Parse(Console.ReadLine());

    Console.WriteLine("Enter product quantity:");
    var updatedProductQuantity = int.Parse(Console.ReadLine());

    var updatedProduct = new Product
    {
        ProductId = defaultProductId,
        ProductName = updatedProductName,
        ProductDescription = updatedProductDescription,
        ProductPrice = updatedProductPrice,
        ProductQuantity = updatedProductQuantity
    };

    Console.WriteLine("Client: Sending request to update product.");
    var updateResponse = await client.UpdateProductAsync(updatedProduct);
    Console.WriteLine($"Client: Received response. Product Updated: {updateResponse.Product.ProductName}");
}
else
{
   
    Console.WriteLine($"Client: Product with ID {defaultProductId} does not exist. Please provide product details to create:");

    Console.WriteLine("Enter product name:");
    var productName = Console.ReadLine();

    Console.WriteLine("Enter product description:");
    var productDescription = Console.ReadLine();

    Console.WriteLine("Enter product price:");
    var productPrice = double.Parse(Console.ReadLine());

    Console.WriteLine("Enter product quantity:");
    var productQuantity = int.Parse(Console.ReadLine());

    var newProduct = new Product
    {
        ProductId = defaultProductId,
        ProductName = productName,
        ProductDescription = productDescription,
        ProductPrice = productPrice,
        ProductQuantity = productQuantity
    };

    Console.WriteLine("Client: Sending request to add new product.");
    var addResponse = await client.AddProductAsync(newProduct);
    Console.WriteLine($"Client: Received response. Product Added: {addResponse.Product.ProductName}");
}
