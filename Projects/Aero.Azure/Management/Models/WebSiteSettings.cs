using System.Collections.Generic;
using Microsoft.Azure.Management.WebSites.Models;

namespace Aero.Azure.Management.Models
{
    public class WebsiteSettings
    {
        public WebsiteSettings()
        {
            ApplicationSettings = new Dictionary<string, string>();
            ConnectionStrings = new Dictionary<string, ConnStringInfo>();
        }

        public string Name { get; set; }
        public string ResourceGroupName { get; set; }
        public Dictionary<string, string> ApplicationSettings { get; set; }
        public Dictionary<string, ConnStringInfo> ConnectionStrings { get; set; }
    }
}
