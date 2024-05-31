using ITI.gRPC.lab.server.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddGrpc();

var app = builder.Build();

app.UseHttpsRedirection();
app.UseRouting();
app.UseGrpcWeb();

app.UseEndpoints(endpoints =>
{
    endpoints.MapGrpcService<InventoryService>().EnableGrpcWeb();
});

app.Run();
