﻿using Aero.Cake.Features.DotNet.Wrappers;
using Aero.Cake.Services;
using Cake.Common.Diagnostics;
using Cake.Common.Tools.DotNet.NuGet.Source;

namespace Aero.Cake.Features.DotNet.Services
{
    public interface INuGetService
    {
        void AddOrUpdateNuGetSource(string name, DotNetNuGetSourceSettings settings);
    }

    public class NuGetService : AbstractService, INuGetService
    {
        private readonly IDotNetWrapper _dotNet;

        public NuGetService(IAeroContext aeroContext, IDotNetWrapper dotNet) : base(aeroContext)
        {
            _dotNet = dotNet;
        }

        public void AddOrUpdateNuGetSource(string name, DotNetNuGetSourceSettings settings)
        {
            var logPrefix = $"{nameof(NuGetService)}.{nameof(AddOrUpdateNuGetSource)}";

            if (_dotNet.NuGetHasSource(name, settings))
            {
                AeroContext.Information($"{logPrefix}, Action: UpdatingSource, Name: {name}");
                _dotNet.NuGetUpdateSource(name, settings);
            }
            else
            {
                AeroContext.Information($"{logPrefix}, Action: AddingSource, Name: {name}");
                _dotNet.NuGetAddSource(name, settings);
            }
        }
    }
}
