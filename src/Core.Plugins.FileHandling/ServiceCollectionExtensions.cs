using Core.FileHandling;
using Core.Ftp;
using Core.Plugins.FileHandling.Delimited;
using Core.Plugins.FileHandling.Excel;
using Core.Plugins.FileHandling.Ftp;
using Core.Plugins.Ftp;
using Core.Providers;
using FluentFTP;
using Microsoft.Extensions.DependencyInjection;
using IFtpClient = Core.Ftp.IFtpClient;

namespace Core.Plugins.FileHandling
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddFileHandlingPlugin(this IServiceCollection services, bool isUseSftp = true)
        {
            services.AddFluentFtp();

            services.AddScoped<IDelimitedFileHandler, GenericParsingDelimitedFileHandler>();
            services.AddScoped<IExcelFileHandler, ClosedXmlExcelHandler>();
            services.AddScoped<IFtpClientFactory, FtpClientFactory>();

            if (isUseSftp)
            {
                services.AddScoped<IFtpClient, RensiSftpClient>();
            }
            else
            {
                services.AddScoped<IFtpClient, SystemFtpClient>();
            }

            return services;
        }

        public static IServiceCollection AddFluentFtp(this IServiceCollection services, string connectionName = "DefaultFtpConnection")
        {
            services.AddScoped<FluentFTP.IFtpClient>(sp =>
            {
                var settings = new FtpClientSettings(sp.GetRequiredService<IConnectionStringProvider>().Get(connectionName));

                return new FtpClient(settings.Host, settings.Username, settings.Password);
            });

            return services;
        }
    }
}
