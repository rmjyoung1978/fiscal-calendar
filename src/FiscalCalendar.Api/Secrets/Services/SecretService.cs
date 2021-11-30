using System;
using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;
using Amazon.SecretsManager.Extensions.Caching;
using NLog;

namespace FiscalCalendar.Api.Secrets.Services
{
    [ExcludeFromCodeCoverage]
    public class SecretService : ISecretService
    {
        private readonly SecretsManagerCache _cache = new SecretsManagerCache();
        private readonly ILogger _logger;

        public SecretService(ILogger logger)
        {
            _logger = logger;
        }

        public async Task<string> GetSecretValue(string secretName)
        {
            if (string.IsNullOrWhiteSpace(secretName))
            {
                _logger.Info($"No AWS Secret name specified for {secretName}");
                return null;
            }

            try
            {
                return await _cache.GetSecretString(secretName);
            }
            catch (Exception ex)
            {
                _logger.Info($"Error reading secret: {secretName}" + ex);
                return null;
            }
        }
    }
}
