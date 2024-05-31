using ITI.gRPC.lab.server.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddGrpc();
builder.Services.AddGrpcReflection(); 

var app = builder.Build();

app.UseHttpsRedirection();
app.UseRouting();
app.UseGrpcWeb();

app.MapGrpcService<InventoryService>();

if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
 

}

app.MapGrpcReflectionService(); 

app.Run();
