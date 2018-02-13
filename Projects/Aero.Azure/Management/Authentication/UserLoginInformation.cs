namespace Aero.Azure.Management.Authentication
{
    /// <summary>
    /// Represents Login Information for a User. 
    /// </summary>
    /// <remarks>
    /// - Taken from the Microsoft Fluent Library
    /// - https://github.com/Azure/azure-libraries-for-net/blob/master/src/ResourceManagement/ResourceManager/Authentication/UserLoginInformation.cs
    /// </remarks>
    public class UserLoginInformation
    {
        public string UserName { get; set; }

        public string Password { get; set; }

        public string ClientId { get; set; }
    }
}
