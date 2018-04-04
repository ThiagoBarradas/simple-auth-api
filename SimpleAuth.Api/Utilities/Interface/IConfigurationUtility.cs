using System;
using System.Globalization;

namespace SimpleAuth.Api.Utilities.Interface
{
    public interface IConfigurationUtility
    {
        string SimpleAuthServiceUrl { get; }

        TimeZoneInfo DefaultTimeZone { get; }

        CultureInfo DefaultCultureInfo { get; }

        int PasswordResetExpiresInHours { get; }

        string DatabaseConnectionHost { get; }

        string DatabaseConnectionPort { get; }

        string DatabaseConnectionUsername { get; }

        string DatabaseConnectionPassword { get; }

        string DatabaseConnectionName { get; }

        string IpInfoApiUrl { get; }

        string HashGap { get; }

        string RollbarAccessToken { get; }

        string RollbarEnvironment { get; }
    }
}
