using FiscalCalendar.Api.Attributes;
using FiscalCalendar.Api.Secrets;
using FiscalCalendar.Api.Secrets.Services;
using FiscalCalendar.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Newtonsoft.Json.Serialization;
using NLog;
using NLog.Web;
using System.Diagnostics.CodeAnalysis;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;
using IConfiguration = Microsoft.Extensions.Configuration.IConfiguration;
using ILogger = NLog.ILogger;

namespace FiscalCalendar.Api
{
    [ExcludeFromCodeCoverage]
    public class Startup
    {
        public IConfiguration Configuration { get; }
        private readonly IWebHostEnvironment _environment;
        private ILogger _logger;

        private const string ApiKeySecretName = "fiscal-calendar-api-key";

        public Startup(IConfiguration configuration, IWebHostEnvironment env)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true)
                .AddEnvironmentVariables();

            if (env.IsDevelopment()) builder.AddUserSecrets<Startup>();
            Configuration = builder.Build();
            _environment = env;
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.Configure<ApiBehaviorOptions>(opt =>
            {
                opt.SuppressModelStateInvalidFilter = true;
                opt.SuppressMapClientErrors = true;
            });

            _logger = LogManager.GetCurrentClassLogger();
            var secrets = GetAllSecrets(new SecretService(_logger), _environment, Configuration).Result;

            services.AddLogging(_ => LogManager.Setup().LoadConfigurationFromAppSettings().GetCurrentClassLogger());
            services.AddSingleton(_logger);
            services.AddSingleton(secrets);
            services.AddSingleton<ICalendarService, CalendarService>();

            services.AddControllers()
                .AddNewtonsoftJson(o =>
                {
                    o.SerializerSettings.Converters.Add(new Newtonsoft.Json.Converters.StringEnumConverter());
                    o.SerializerSettings.ContractResolver = new DefaultContractResolver { NamingStrategy = new DefaultNamingStrategy() };
                });

            services.AddMvc(mvc =>
            {
                mvc.Filters.Add(typeof(ModelStateValidationFilterAttribute));
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
                app.UseDeveloperExceptionPage();

            app.UseHttpsRedirection();
            app.UseRouting();
            app.UseAuthorization();
            app.UseEndpoints(endpoints => { endpoints.MapControllers(); });
        }

        public static async Task<IFiscalCalendarSecrets> GetAllSecrets(ISecretService secretService, IHostEnvironment environment, IConfiguration configuration)
        {
            if (environment.IsDevelopment())
                return configuration.GetSection("FiscalCalendarSecrets").Get<FiscalCalendarSecrets>();

            return new FiscalCalendarSecrets
            {
                AuthorizationKeySecret = await secretService.GetSecretValue(ApiKeySecretName)
            };
        }
    }
}
