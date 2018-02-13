using System;
using Microsoft.IdentityModel.Clients.ActiveDirectory;

namespace Aero.Azure.Management.Authentication
{
    public interface IAzureCredentialFactory
    {
        AzureCredentials FromDevice(string clientId, string tenantId, AzureEnvironment environment = null, Func<DeviceCodeResult, bool> deviceCodeFlowHandler = null);

        AzureCredentials FromServicePrincipal(string clientId, string clientSecret, string tenantId, AzureEnvironment environment = null);
    }

    /// <summary>
    /// Creates and returns an instance of AzureCredentials
    /// </summary>
    /// <remarks>
    /// - Taken from the Microsoft Fluent Library
    /// - https://github.com/Azure/azure-libraries-for-net/blob/master/src/ResourceManagement/ResourceManager/Authentication/AzureCredentialsFactory.cs
    /// </remarks>
    public class AzureCredentialFactory : IAzureCredentialFactory
    {
        /// <summary>
        /// Creates a credentials object through device flow.
        /// </summary>
        /// <param name="clientId">the client ID of the application</param>
        /// <param name="tenantId">the tenant ID or domain</param>
        /// <param name="environment">the environment to authenticate to</param>
        /// <param name="deviceCodeFlowHandler">a user defined function to handle device flow</param>
        /// <returns>an authenticated credentials object</returns>
        public AzureCredentials FromDevice(string clientId, string tenantId, AzureEnvironment environment = null, Func<DeviceCodeResult, bool> deviceCodeFlowHandler = null)
        {
            AzureCredentials credentials = new AzureCredentials(new DeviceCredentialInformation
            {
                ClientId = clientId,
                DeviceCodeFlowHandler = deviceCodeFlowHandler
            }, tenantId, environment ?? AzureEnvironment.AzureGlobalCloud);
            return credentials;
        }

        /// <summary>
        /// Creates a credentials object from a service principal.
        /// </summary>
        /// <param name="clientId">the client ID of the application the service principal is associated with</param>
        /// <param name="clientSecret">the secret for the client ID</param>
        /// <param name="tenantId">the tenant ID or domain the application is in</param>
        /// <param name="environment">the environment to authenticate to</param>
        /// <returns>an authenticated credentials object</returns>
        public AzureCredentials FromServicePrincipal(string clientId, string clientSecret, string tenantId, AzureEnvironment environment = null)
        {
            return new AzureCredentials(new ServicePrincipalLoginInformation
            {
                ClientId = clientId,
                ClientSecret = clientSecret
            }, tenantId, environment ?? AzureEnvironment.AzureGlobalCloud);
        }
    }
}
