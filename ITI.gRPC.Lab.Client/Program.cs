using Grpc.Net.Client;
using ITI.gRPC.lab.server.Protos;
using System;
using System.Threading.Tasks;

var channel = GrpcChannel.ForAddress("https://localhost:7147");
var client = new InventoryService.InventoryServiceClient(channel);

var reply = await client.GetProductByIdAsync(new Id { Id_ = 1 });
Console.WriteLine("Product Existed: " + reply.IsExistd);
Console.WriteLine(
    await client.AddProductAsync(new Product
    {
        Id = 4,
        Name = "P4",
        Descripton = "D4",
        Quantity = 4
    })
    );
Console.WriteLine(
    await client.UpdateProductAsync(new Product
    {
        Id = 4,
        Name = "P4",
        Descripton = "D4",
        Quantity = 5
    })
       );
Console.WriteLine( await client.GetProductByIdAsync(new Id { Id_ = 4 }));
 
