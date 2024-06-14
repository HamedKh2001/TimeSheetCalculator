using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Serilog;
using Serilog.Events;
using Serilog.Exceptions;
using Serilog.Sinks.Elasticsearch;
using System;
using System.Collections.Generic;

namespace SharedKernel.Extensions
{
    public static class LoggingConfigurationExtention
    {
        public static void AddMySerilogWithELK(this IHostBuilder host, string applicationName)
        {
            host.UseSerilog((context, configuration)
                    => ConfigureLogger(context, configuration, applicationName));
        }

        private static Action<HostBuilderContext, LoggerConfiguration, string> ConfigureLogger =>
        (context, configuration, applicationName) =>
        {
            #region Enriching Logger Context
            var env = context.HostingEnvironment;
            configuration.Enrich.FromLogContext()
                .Enrich.WithProperty("ApplicationName", applicationName)
                .Enrich.WithProperty("Environment", env.EnvironmentName)
                .Enrich.FromLogContext()
                .Enrich.WithExceptionDetails();
            #endregion
            if (env.IsDevelopment())
            {
                configuration.WriteTo.Console().MinimumLevel.Information();
            }


            #region ElasticSearch Configuration.
            var elasticUrl = context.Configuration.GetValue<string>("ElasticConfiguration:ElasticSearchUri");
            if (!string.IsNullOrEmpty(elasticUrl))
            {
                configuration.WriteTo.Elasticsearch(
            //new ElasticsearchSinkOptions(new List<Uri>() { new Uri("http://localhost:9200"), new Uri("http://localhost:9202") })
            new ElasticsearchSinkOptions(new List<Uri>() { new Uri(elasticUrl) })
            {
                AutoRegisterTemplate = true,
                AutoRegisterTemplateVersion = AutoRegisterTemplateVersion.ESv7,
                NumberOfShards = 6,
                NumberOfReplicas = 1,
                IndexFormat = context.Configuration.GetValue<string>("ElasticConfiguration:Index"),
                MinimumLogEventLevel = LogEventLevel.Information,
            });
            }
            #endregion
        };
    }
}
