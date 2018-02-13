namespace Aero.Azure.Management.Authentication
{
    /// <summary>
    /// Represents Login Information for a Service Principal. 
    /// </summary>
    /// <remarks>
    /// - Taken from the Microsoft Fluent Library
    /// - https://github.com/Azure/azure-libraries-for-net/blob/master/src/ResourceManagement/ResourceManager/Authentication/ServicePrincipalLoginInformation.cs
    /// </remarks>
    public class ServicePrincipalLoginInformation
    {
        public string ClientId { get; set; }

        public string ClientSecret { get; set; }

        public byte[] Certificate { get; set; }

        public string CertificatePassword { get; set; }
    }
}
