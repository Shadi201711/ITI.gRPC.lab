using Grpc.Core;
using ITI.gRPC.lab.server.Protos;
using System.Collections.Concurrent;
using System.Threading.Tasks;

namespace ITI.gRPC.lab.server.Services
{
    public class InventoryService : Inventory.InventoryBase
    {
        private static readonly ConcurrentDictionary<string, Product> Products = new();

        public override Task<ProductResponse> GetProductById(ProductRequest request, ServerCallContext context)
        {
            var response = new ProductResponse();
            if (Products.TryGetValue(request.ProductId, out var product))
            {
                response.Exists = true;
                response.Product = product;
            }
            else
            {
                response.Exists = false;
            }
            return Task.FromResult(response);
        }

        public override Task<ProductResponse> AddProduct(Product request, ServerCallContext context)
        {
            Products[request.ProductId] = request;
            return Task.FromResult(new ProductResponse { Exists = true, Product = request });
        }

        public override Task<ProductResponse> UpdateProduct(Product request, ServerCallContext context)
        {
            if (Products.ContainsKey(request.ProductId))
            {
                Products[request.ProductId] = request;
                return Task.FromResult(new ProductResponse { Exists = true, Product = request });
            }
            return Task.FromResult(new ProductResponse { Exists = false });
        }
    }
}
