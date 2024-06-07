using Google.Protobuf.WellKnownTypes;
using Grpc.Core;
using ITI.gRPC.lab.server.Protos;
using Microsoft.AspNetCore.Authorization;
using System.Collections.Concurrent;
using System.Threading.Tasks;
using static ITI.gRPC.lab.server.Protos.InventoryService;

namespace ITI.gRPC.lab.server.Services
{
    public class InventoryService : InventoryServiceBase
    {

        public List<Product> Products { get; set; }
        public List<ProductToAdd> ProductsToAdd { get; set; } = new List<ProductToAdd>();


        public InventoryService()
        {
            Products = new List<Product>()
            {
                new Product{Id=1,Name="P1",Descripton="D1",Quantity=1},
                new Product{Id=2,Name="P2",Descripton="D2",Quantity=10},
                new Product{Id=3,Name="P3",Descripton="D3",Quantity=20}
            };

            ProductsToAdd = new List<ProductToAdd>()
            {
                new ProductToAdd{Id = 1,Name="P1",Quantity = 1,Category = Category.Laptops,ExpiredDate = Timestamp.FromDateTime(DateTime.UtcNow) },
                new ProductToAdd{Id = 2,Name="P2",Quantity = 21,Category = Category.Modilers,ExpiredDate = Timestamp.FromDateTime(DateTime.UtcNow) },
                new ProductToAdd{Id = 3,Name="P3",Quantity = 2,Category = Category.Foods,ExpiredDate = Timestamp.FromDateTime(DateTime.UtcNow) },
                new ProductToAdd{Id = 4,Name="P4",Quantity = 3,Category = Category.Modilers,ExpiredDate = Timestamp.FromDateTime(DateTime.UtcNow) }
            };
        }

        [Authorize(AuthenticationSchemes = Consts.ApiKeySchemeName)]
        public override async Task<IsProductExisted> GetProductById(Id request, ServerCallContext context)
        {
            var prodcut = Products.FirstOrDefault(p => p.Id == request.Id_);

            if (prodcut != null)
            {
                return await Task.FromResult(new IsProductExisted
                {
                    IsExistd = true,
                });
            }

            return await Task.FromResult(new IsProductExisted
            {
                IsExistd = false,
            });
        }

        [Authorize(AuthenticationSchemes = Consts.ApiKeySchemeName)]
        public override async Task<Product> AddProduct(Product request, ServerCallContext context)
        {
            Products.Add(request);

            return await Task.FromResult(request);
        }

        [Authorize(AuthenticationSchemes = Consts.ApiKeySchemeName)]
        public override async Task<Product> UpdateProduct(Product request, ServerCallContext context)
        {
            var product = Products.FirstOrDefault(p => p.Id == request.Id);

            product.Name = request.Name;
            product.Descripton = request.Descripton;
            product.Quantity = request.Quantity;

            return await Task.FromResult(product);
        }

        [Authorize(AuthenticationSchemes = Consts.ApiKeySchemeName)]
        public override async Task<NumberOfInsertedProducts> AddBulkProducts(IAsyncStreamReader<ProductToAdd> requestStream, ServerCallContext context)
        {
            int count = 0;
            await foreach (var request in requestStream.ReadAllAsync())
            {
                ProductsToAdd.Add(request);
                ++count;
            }

            return await Task.FromResult(new NumberOfInsertedProducts { Count = count });
        }

        [Authorize(AuthenticationSchemes = Consts.ApiKeySchemeName)]
        public override async Task GetProductReport(Empty request, IServerStreamWriter<ProductToAdd> responseStream, ServerCallContext context)
        {
            foreach (var item in ProductsToAdd)
            {
                await responseStream.WriteAsync(item);
            }

            await Task.CompletedTask;
        }
    }
}
