using Microsoft.AspNetCore.Builder;
using PermissionServer.Common.Internal;

namespace PermissionServer
{
    public static class IApplicationBuilderExtensions
    {
        public static void UsePermissionServer(this IApplicationBuilder app)
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