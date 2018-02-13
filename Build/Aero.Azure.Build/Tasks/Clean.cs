using Cake.Common.IO;
using Cake.Frosting;

namespace Aero.Build.Tasks
{ 
    public class Clean : FrostingTask<Context>
    {
        public override void Run(Context context)
        {
            var directories = context.GetDirectories($"{context.ProjectsPath}/**/bin")
                + context.GetDirectories($"{context.ProjectsPath}/**/obj");

            foreach (var directory in directories)
            {
                context.CleanDirectory(directory);
            }
        }
    }
}
