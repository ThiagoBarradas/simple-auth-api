using Microsoft.Extensions.Configuration;
using SimpleAuth.Api.Utilities.Interface;
using System;
using System.Globalization;

namespace SimpleAuth.Api.Utilities
{
    public class ConfigurationUtility : IConfigurationUtility
    {
        public IConfigurationRoot ConfigurationRoot { get; set; }

        public ConfigurationUtility(IConfigurationRoot configurationRoot)
        {
            this.ConfigurationRoot = configurationRoot;
        }

        public string SimpleAuthServiceUrl => this.ConfigurationRoot["Application:Url"];

        public TimeZoneInfo DefaultTimeZone => TimeZoneInfo.FindSystemTimeZoneById(this.ConfigurationRoot["Default:TimeZone"]);

        public CultureInfo DefaultCultureInfo => CultureInfo.GetCultureInfo(this.ConfigurationRoot["Default:CultureInfo"]);

        public int PasswordResetExpiresInHours => Int32.Parse(this.ConfigurationRoot["Default:PasswordResetExpiresInHours"]);

        public string DatabaseConnectionHost => this.ConfigurationRoot["DatabaseConnection:Host"];

        public string DatabaseConnectionPort => this.ConfigurationRoot["DatabaseConnection:Port"];

        public string DatabaseConnectionUsername => this.ConfigurationRoot["DatabaseConnection:Username"];

        public string DatabaseConnectionPassword => this.ConfigurationRoot["DatabaseConnection:Password"];

        public string DatabaseConnectionName => this.ConfigurationRoot["DatabaseConnection:Name"];

        public string IpInfoApiUrl => this.ConfigurationRoot["IpInfo:ApiUrl"];

        public string HashGap => this.ConfigurationRoot["HashGap"];
    }
}