
using ITI.gRPC.lab.Clientt.Services;
using ITI.gRPC.lab.Clientt;
using ITI.gRPC.lab.server;
using ITI.gRPC.lab.server.Protos;



namespace ITI.gRPC.lab.Clientt
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            var _configuration = builder.Configuration;
            // Add services to the container.

            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddScoped<IApiKeyProviderService, ApiKeyProviderService>();

            _ = builder.Services.AddGrpcClient<InventoryService.InventoryServiceClient>(options =>
            {
                var address = _configuration.GetValue<string>(Consts.GrpcServiceAddressSettingName);
                options.Address = new Uri(address);
            }).AddCallCredentials((context, metadata, serviceProvider) =>
            {
                var apiKeyProvider = serviceProvider.GetRequiredService<IApiKeyProviderService>();
                var apiKey = apiKeyProvider.GetApiKey();
                metadata.Add(Consts.ApiKeyHeaderName, apiKey);
                return Task.CompletedTask;
            });
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseAuthorization();

            app.MapControllers();

            app.Run();
        }
    }

}