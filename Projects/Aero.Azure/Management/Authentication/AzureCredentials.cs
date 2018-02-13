using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.IdentityModel.Clients.ActiveDirectory;
using Microsoft.Rest;
using Microsoft.Rest.Azure.Authentication;

namespace Aero.Azure.Management.Authentication
{
    /// <inheritdoc />
    /// <summary>
    /// Implements ServiceClientCrednentials for Azure
    /// </summary>
    /// <remarks>
    /// - Taken from the Microsoft Fluent Library
    /// - https://github.com/Azure/azure-libraries-for-net/blob/master/src/ResourceManagement/ResourceManager/Authentication/AzureCredentials.cs
    /// </remarks>
    public class AzureCredentials : ServiceClientCredentials
    {
        private readonly IDictionary<Uri, ServiceClientCredentials> _credentialsCache;
        private readonly DeviceCredentialInformation _deviceCredentialInformation;
        private readonly ServicePrincipalLoginInformation _servicePrincipalLoginInformation;
        private readonly UserLoginInformation _userLoginInformation;



        //Not supported in .Net Core
        //public AzureCredentials(UserLoginInformation userLoginInformation, string tenantId, AzureEnvironment environment)
        //    : this(tenantId, environment)
        //{
        //    _userLoginInformation = userLoginInformation;
        //}

        public AzureCredentials(ServicePrincipalLoginInformation servicePrincipalLoginInformation, string tenantId, AzureEnvironment environment)
            : this(tenantId, environment)
        {
            _servicePrincipalLoginInformation = servicePrincipalLoginInformation;
        }

        public AzureCredentials(DeviceCredentialInformation deviceCredentialInformation, string tenantId, AzureEnvironment environment)
            : this(tenantId, environment)
        {
            _deviceCredentialInformation = deviceCredentialInformation;

        }

        private AzureCredentials(string tenantId, AzureEnvironment environment)
        {
            TenantId = tenantId;
            Environment = environment;
            _credentialsCache = new Dictionary<Uri, ServiceClientCredentials>();
        }



        public string DefaultSubscriptionId { get; private set; }

        public string TenantId { get; }

        public string ClientId
        {
            get
            {
                if (_deviceCredentialInformation != null)
                {
                    return _deviceCredentialInformation.ClientId;
                }
                return _userLoginInformation?.ClientId ?? _servicePrincipalLoginInformation?.ClientId;
            }
        }

        public AzureEnvironment Environment { get; }



        public AzureCredentials WithDefaultSubscription(string subscriptionId)
        {
            DefaultSubscriptionId = subscriptionId;
            return this;
        }

        public override async Task ProcessHttpRequestAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            var adSettings = new ActiveDirectoryServiceSettings
            {
                AuthenticationEndpoint = new Uri(Environment.AuthenticationEndpoint),
                TokenAudience = new Uri(Environment.ManagementEndpoint),
                ValidateAuthority = true
            };

            var url = request.RequestUri.ToString();
            if (url.StartsWith(Environment.GraphEndpoint, StringComparison.OrdinalIgnoreCase))
            {
                adSettings.TokenAudience = new Uri(Environment.GraphEndpoint);
            }

            if (!_credentialsCache.ContainsKey(adSettings.TokenAudience))
            {
                if (_servicePrincipalLoginInformation != null)
                {
                    if (_servicePrincipalLoginInformation.ClientSecret != null)
                    {
                        _credentialsCache[adSettings.TokenAudience] = await ApplicationTokenProvider.LoginSilentAsync(
                            TenantId, _servicePrincipalLoginInformation.ClientId, _servicePrincipalLoginInformation.ClientSecret, adSettings, TokenCache.DefaultShared);
                    }
                    else
                    {
                        _credentialsCache[adSettings.TokenAudience] = await ApplicationTokenProvider.LoginSilentAsync(
                            TenantId, _servicePrincipalLoginInformation.ClientId, _servicePrincipalLoginInformation.Certificate, _servicePrincipalLoginInformation.CertificatePassword, TokenCache.DefaultShared);
                    }
                }
                //else if (_userLoginInformation != null) //Not supported in .Net Core. UserTokenProvider.LoginSilentAsync does not exist
                //{
                //    _credentialsCache[adSettings.TokenAudience] = await UserTokenProvider.LoginSilentAsync(
                //        _userLoginInformation.ClientId, TenantId, _userLoginInformation.UserName,
                //        _userLoginInformation.Password, adSettings, TokenCache.DefaultShared);
                //}
                else if (_deviceCredentialInformation != null)
                {
                    _credentialsCache[adSettings.TokenAudience] = await UserTokenProvider.LoginByDeviceCodeAsync(
                        _deviceCredentialInformation.ClientId, TenantId, adSettings, TokenCache.DefaultShared, _deviceCredentialInformation.DeviceCodeFlowHandler);
                }

            }
            await _credentialsCache[adSettings.TokenAudience].ProcessHttpRequestAsync(request, cancellationToken);
        }
    }
}
