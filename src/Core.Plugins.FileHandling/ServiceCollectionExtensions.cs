using Core.FileHandling;
using Core.Ftp;
using Core.Plugins.FileHandling.Delimited;
using Core.Plugins.FileHandling.Excel;
using Core.Plugins.FileHandling.Ftp;
using Core.Plugins.Ftp;
using Microsoft.Extensions.DependencyInjection;

namespace Core.Plugins.FileHandling
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddFileHandlingPlugin(this IServiceCollection services, bool isUseSftp = true)
        {
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
    }
}
