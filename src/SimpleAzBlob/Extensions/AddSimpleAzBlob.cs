using Microsoft.Extensions.DependencyInjection;
using S4S.Libraries.Storage.BlobStorage;
using SimpleAzBlob.Clients;
using SimpleAzBlob.Interface;
using SimpleAzBlob.Models;
using System;
using System.Collections.Generic;
using System.Linq;
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

            if (!services.Any(a => a.ServiceType == typeof(SimpleAzBlobContainerManager<AzureStorageAccount>)))
            services.AddSingleton<SimpleAzBlobContainerManager<AzureStorageAccount>>();
            if (!services.Any(a => a.ServiceType == typeof(ISimpleAzBlobClient)))
                services.AddSingleton<ISimpleAzBlobClient, SimpleAzBlobClient>();
            return services;
        }

        /// <summary>
        /// Service Extension to Inject Blob Storage Into Service Collection Dependancy Injection.
        /// 
        /// REQUIRES INJECTED IConfiguration, ILogger.
        /// </summary>
        /// <typeparam name="T">The label type to differentiate multiple ISimpleAzBlobClient in dependancy injection. THIS IS ALSO THE NAME OF YOUR CONNECTION STRING</typeparam>
        /// <param name="services"></param>
        /// <returns></returns>
        public static IServiceCollection AddSimpleAzBlob<AccountType>(this IServiceCollection services)
        {

            if (!services.Any(a => a.ServiceType == typeof(SimpleAzBlobContainerManager<AccountType>)))
                services.AddSingleton<SimpleAzBlobContainerManager<AccountType>>();
            if (!services.Any(a => a.ServiceType == typeof(ISimpleAzBlobClient<AccountType>)))
                services.AddSingleton<ISimpleAzBlobClient<AccountType>, SimpleAzBlobClient<AccountType>>();
            return services;
        }
    }
}
