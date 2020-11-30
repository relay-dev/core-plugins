using Core.FileHandling;
using Core.Plugins.Configuration;
using Core.Plugins.FileHandling.Delimited;
using Core.Plugins.FileHandling.Excel;
using Core.Plugins.FileHandling.Ftp;
using Core.Plugins.Utilities.Ftp;
using Core.Providers;
using Core.Utilities.Ftp;
using FluentFTP;
using Microsoft.Extensions.DependencyInjection;
using IFtpClient = Core.Utilities.Ftp.IFtpClient;

namespace Core.Plugins.FileHandling
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddFileHandlingPlugin(this IServiceCollection services, PluginConfiguration pluginConfiguration, bool isUseSftp = true)
        {
            services.AddFluentFtp(pluginConfiguration);

            services.Add<IDelimitedFileHandler, GenericParsingDelimitedFileHandler>(pluginConfiguration.ServiceLifetime);
            services.Add<IExcelFileHandler, ClosedXmlExcelHandler>(pluginConfiguration.ServiceLifetime);
            services.Add<IFtpClientFactory, FtpClientFactory>(pluginConfiguration.ServiceLifetime);

            if (isUseSftp)
            {
                services.Add<IFtpClient, RensiSftpClient>(pluginConfiguration.ServiceLifetime);
            }
            else
            {
                services.Add<IFtpClient, SystemFtpClient>(pluginConfiguration.ServiceLifetime);
            }

            return services;
        }

        public static IServiceCollection AddFluentFtp(this IServiceCollection services, PluginConfiguration pluginConfiguration, string connectionName = "DefaultFtpConnection")
        {
            services.Add<FluentFTP.IFtpClient>(sp =>
            {
                var settings = new FtpClientSettings(sp.GetRequiredService<IConnectionStringProvider>().Get(connectionName));

                return new FtpClient(settings.Host, settings.Username, settings.Password);
            }, pluginConfiguration.ServiceLifetime);

            return services;
        }
    }
}
