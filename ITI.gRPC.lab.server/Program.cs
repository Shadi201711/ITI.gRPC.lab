using ITI.gRPC.lab.server.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddGrpc();
builder.Services.AddGrpcReflection(); // Add this line to enable server reflection

var app = builder.Build();

app.UseHttpsRedirection();
app.UseRouting();
app.UseGrpcWeb();

app.MapGrpcService<InventoryService>();

if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
   // app.UseWebAssemblyDebugging();

}

app.MapGrpcReflectionService(); // Add this line to enable server reflection

app.Run();
