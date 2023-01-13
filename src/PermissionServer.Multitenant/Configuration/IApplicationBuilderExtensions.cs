using Microsoft.AspNetCore.Builder;
using PermissionServer.Common.Configuration;
using PermissionServer.Multitenant.Grpc;

namespace PermissionServer.Multitenant.Configuration
{
    public static class IApplicationBuilderExtensions
    {
        public static void AddPermissionServer(this IApplicationBuilder app)
        {
            var authoritySettings = app
                .ApplicationServices.GetService(typeof(IAuthoritySettings)) as IAuthoritySettings;
            if (authoritySettings.IsAuthority)
            {
                app.UseEndpoints(e =>
                {
                    e.MapGrpcService<RemotePermissionAuthorizeService>();
                });
            }
        }
    }
}