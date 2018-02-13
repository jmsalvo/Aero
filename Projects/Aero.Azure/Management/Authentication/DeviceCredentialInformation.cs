using System;
using Microsoft.IdentityModel.Clients.ActiveDirectory;

namespace Aero.Azure.Management.Authentication
{
    public class DeviceCredentialInformation
    {
        public string ClientId { get; set; }

        public Func<DeviceCodeResult, bool> DeviceCodeFlowHandler { get; set; }
    }
}
