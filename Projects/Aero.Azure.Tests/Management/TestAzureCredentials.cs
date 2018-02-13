using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Aero.Azure.Management.Authentication;
using Microsoft.Azure.Test.HttpRecorder;

namespace Aero.Azure.Management
{
    public class TestAzureCredentials : AzureCredentials
    {
        public TestAzureCredentials(ServicePrincipalLoginInformation servicePrincipalLoginInformation,
            string tenantId, AzureEnvironment environment)
            : base(servicePrincipalLoginInformation, tenantId, environment)
        {
        }

        public override async Task ProcessHttpRequestAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            if (HttpMockServer.Mode == HttpRecorderMode.Playback)
            {
                return;
            }
            await base.ProcessHttpRequestAsync(request, cancellationToken);
        }
    }
}
