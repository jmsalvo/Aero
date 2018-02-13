using Cake.Common.Diagnostics;
using Cake.Common.IO;
using Cake.Common.Xml;
using Cake.Core;
using Cake.Core.IO;

namespace Aero.Cake.Services
{ 
    public interface IVersionSevice
    {
        void UpdateFiles(string version, string projectPath);
    }

    public class VersionService : AbstractService, IVersionSevice
    {
        public VersionService(ICakeContext cakeContext, IAeroLogger<VersionService> logger) : base(cakeContext, logger)
        {

        }

        /// <summary>
        /// Goes thru CsProj files and if they have a AssemblyVersion attribute, updates it
        /// </summary>
        /// <remarks>Keeping version as a parameter instead of looking for the version argument as this is 
        /// something that could be pulled out into a NuGet package at some point</remarks>
        public void UpdateFiles(string version, string projectPath)
        {
            //We'll keep this around for dealing with Azure Functions as I think that has to be Full Framework
            //var assemblyInfoFiles = context.GetFiles($"{context.ProjectsPath}/**/properties/AssemblyInfo.cs");

            var csprojFiles = CakeContext.GetFiles($"{projectPath}/**/*.csproj");

            foreach(var f in csprojFiles)
            {
                UpdateCsProjFile(f, version);
            }

        }

        private void UpdateCsProjFile(FilePath filePath, string version)
        {
            if (!CakeContext.XmlPeek(filePath, "/Project/@Sdk").StartsWith("Microsoft.NET.Sdk"))
                return;

            //We either have Major.Minor.Build.Rev or yyMMdd.Rev from VSTS build
            //When we have yyMMdd.Rev, then we change it to yy.MM.dd.Rev
            var segments = version.Split('.');
            if (segments.Length == 2)
            {
                version = $"{segments[0].Substring(0,2)}.{segments[0].Substring(2,2)}.{segments[0].Substring(4,2)}.{segments[1]}";
            }

            CakeContext.Information($"Update csproj for version. Version: {version}, File: {filePath}");
            CakeContext.XmlPoke(filePath, "/Project/PropertyGroup/AssemblyVersion", version);
            CakeContext.XmlPoke(filePath, "/Project/PropertyGroup/FileVersion", version);
            CakeContext.XmlPoke(filePath, "/Project/PropertyGroup/Version", version);
        }
    }
}
