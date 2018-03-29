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

        public string SimpleAuthServiceUrl => this.ConfigurationRoot["ApplicationUrl"];

        public TimeZoneInfo DefaultTimeZone => TimeZoneInfo.FindSystemTimeZoneById(this.ConfigurationRoot["DefaultTimeZone"]);

        public CultureInfo DefaultCultureInfo => CultureInfo.GetCultureInfo(this.ConfigurationRoot["DefaultCultureInfo"]);

        public int PasswordResetExpiresInHours => Int32.Parse(this.ConfigurationRoot["DefaultPasswordResetExpiresInHours"]);

        public string DatabaseConnectionHost => this.ConfigurationRoot["DatabaseConnectionHost"];

        public string DatabaseConnectionPort => this.ConfigurationRoot["DatabaseConnectionPort"];

        public string DatabaseConnectionUsername => this.ConfigurationRoot["DatabaseConnectionUsername"];

        public string DatabaseConnectionPassword => this.ConfigurationRoot["DatabaseConnectionPassword"];

        public string DatabaseConnectionName => this.ConfigurationRoot["DatabaseConnectionName"];

        public string IpInfoApiUrl => this.ConfigurationRoot["IpInfoApiUrl"];

        public string HashGap => this.ConfigurationRoot["HashGap"];
    }
}