using Aero.Cake.CupCakes;
using Cake.Common.Tools.DotNetCore.Test;
using Cake.Frosting;
using Microsoft.Extensions.DependencyInjection;

namespace Aero.Build.Tasks
{
    public class UnitTest : FrostingTask<Context>
    {
        public override void Run(Context context)
        {
            var dotNetCore = context.ServiceProvider.GetService<IDotNetCoreCupCake>();

            var testSettings = new DotNetCoreTestSettings
            {
                Configuration = context.Configuration,
                Logger = "trx"
            };

            dotNetCore.Test($"{context.ProjectsPath}/Aero.Common.Azure.Tests/Aero.Common.Azure.Tests.csproj", testSettings);
        }
    }
}
