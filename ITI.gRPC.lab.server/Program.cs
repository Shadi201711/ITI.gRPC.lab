using ITI.gRPC.lab.server.Handler;
using ITI.gRPC.lab.server.Services;
using ITI.gRPC.lab.server;
using ITI.gRPC.lab.server.Handler;
using ITI.gRPC.lab.server.Services;
using Microsoft.AspNetCore.Authentication;

namespace ITI.gRPC.lab.server
{
    public class Program
    {
        const string ApiKeySchemeName = "X-Api-Key";
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Services.AddHttpContextAccessor();
            builder.Services.AddGrpc();
            builder.Services.AddScoped<IApiKeyAuthenticationService, ApiKeyAuthenticationServicecs>();
            builder.Services.AddAuthentication(o =>
            {
                o.DefaultAuthenticateScheme = Consts.ApiKeySchemeName;
            }).AddScheme<AuthenticationSchemeOptions, ApiKeyAuthonticationHandler>(Consts.ApiKeySchemeName, configureOptions => { });
            builder.Services.AddAuthorization();

            var app = builder.Build();


            app.UseHttpsRedirection();
            app.UseAuthentication();
            app.UseAuthorization();

            app.MapGrpcService<InventoryService>();
            app.Run();
        }
    }
}
