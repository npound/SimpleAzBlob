using Microsoft.Extensions.DependencyInjection;
using S4S.Libraries.Storage.BlobStorage;
using SimpleAzBlob.Clients;
using SimpleAzBlob.Interface;
using System;
using System.Collections.Generic;
using System.Text;

namespace SimpleAzBlob.Extensions
{
    public static class Extensions
    {
        /// <summary>
        /// Service Extension to Inject Blob Storage Into Service Collection Dependancy Injection.
        /// 
        /// REQUIRES INJECTED IConfiguration, ILogger.
        /// </summary>
        /// <param name="services"></param>
        /// <returns></returns>
        public static IServiceCollection AddSimpleAzBlob(this IServiceCollection services)
        {
            services.AddSingleton<SimpleAzBlobContainerManager>();
            services.AddSingleton<ISimpleAzBlobClient, SimpleAzBlobClient>();

            return services;
        }
    }
}
