using Aero.Azure.ArmTemplate;
using Aero.Azure.Management;
using Aero.Azure.Management.Authentication;
using Microsoft.Extensions.DependencyInjection;

namespace Aero.Azure
{
    public static class AeroCommonAzureBindings
    {
        public static void Bind(IServiceCollection services)
        {
            services
                .AddSingleton<IAzureCredentialFactory, AzureCredentialFactory>()
                .AddTransient<IAzureManagementService, AzureManagementService>()
                .AddTransient<IResourceManagerService, ResourceManagerService>()
                .AddSingleton<ITemplateBuilder, TemplateBuilder>()
                .AddTransient<IWebsiteService, WebsiteService>();

        }
    }
}
