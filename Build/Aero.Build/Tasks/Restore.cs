using Cake.Common.Tools.DotNetCore;
using Cake.Common.Tools.DotNetCore.Restore;
using Cake.Frosting;

namespace Aero.Build.Tasks
{
    public class Restore : FrostingTask<Context>
    {
        public override void Run(Context context)
        {
            context.DotNetCoreRestore(context.SolutionFile, new DotNetCoreRestoreSettings
            {
                Sources = new[] {
                    "https://api.nuget.org/v3/index.json",
                    "https://www.myget.org/F/cake/api/v3/index.json" //For Cake Frosting Alpha
                }
            });
        }
    }
}
