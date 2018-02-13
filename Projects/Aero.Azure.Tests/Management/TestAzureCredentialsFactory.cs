using System;
using Aero.Azure.Management.Authentication;
using Microsoft.Azure.Test.HttpRecorder;
using Microsoft.IdentityModel.Clients.ActiveDirectory;
using Microsoft.Rest.ClientRuntime.Azure.TestFramework;

namespace Aero.Azure.Management
{
    public class TestAzureCredentialsFactory : IAzureCredentialFactory
    {
        public AzureCredentials FromDevice(string clientId, string tenantId, AzureEnvironment environment,
            Func<DeviceCodeResult, bool> deviceCodeFlowHandler = null)
        {
            throw new NotImplementedException();
        }

        public AzureCredentials FromServicePrincipal(string clientId, string clientSecret, string tenantId, AzureEnvironment environment)
        {
            var env = new AzureEnvironment
            {
                AuthenticationEndpoint = "https://www.contoso.com",
                ManagementEndpoint = "https://www.contoso.com",
                ResourceManagerEndpoint = "https://www.contoso.com",
                GraphEndpoint = "https://www.contoso.com"
            };

            if (HttpMockServer.Mode == HttpRecorderMode.Playback)
            {
                

                AzureCredentials credentials = new TestAzureCredentials(
                    new ServicePrincipalLoginInformation
                    {
                        ClientId = HttpMockServer.Variables.ContainsKey(ConnectionStringKeys.AADTenantKey) ?
                            HttpMockServer.Variables[ConnectionStringKeys.ServicePrincipalKey] : "servicePrincipalNotRecorded",
                        ClientSecret = null
                    },
                    HttpMockServer.Variables.ContainsKey(ConnectionStringKeys.AADTenantKey) ?
                        HttpMockServer.Variables[ConnectionStringKeys.AADTenantKey] : "tenantIdNotRecorded", env);
                credentials.WithDefaultSubscription(
                    HttpMockServer.Variables.ContainsKey(ConnectionStringKeys.SubscriptionIdKey) ?
                        HttpMockServer.Variables[ConnectionStringKeys.SubscriptionIdKey] : "subscriptionIdNotRecorded");

                return credentials;
            }

            HttpMockServer.Variables[ConnectionStringKeys.ServicePrincipalKey] = clientId;
            HttpMockServer.Variables[ConnectionStringKeys.AADTenantKey] = tenantId;
            HttpMockServer.Variables[ConnectionStringKeys.SubscriptionIdKey] = Guid.Parse("{15093D29-3166-4DD3-A8A4-1F62B450AB3E}").ToString();

            return new TestAzureCredentials(new ServicePrincipalLoginInformation{ClientId = clientId, ClientSecret = "TestSecret"}, tenantId, env);
        }
    }
}
