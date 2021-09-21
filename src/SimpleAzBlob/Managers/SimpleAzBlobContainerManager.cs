using Azure.Storage.Blobs;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Concurrent;
using System.Threading.Tasks;

namespace SimpleAzBlob.Clients
{
    public class SimpleAzBlobContainerManager
    {
        private const string AzureStorageAccount = "AzureStorageAccount";
        private readonly IConfiguration configuration;
        private readonly ILogger<SimpleAzBlobContainerManager> logger;
        private static ConcurrentDictionary<string, BlobContainerClient> BlobContainerClients = new ConcurrentDictionary<string, BlobContainerClient>();

        public SimpleAzBlobContainerManager(IConfiguration configuration, ILogger<SimpleAzBlobContainerManager> logger)
        {
            this.configuration = configuration;
            this.logger = logger;
        }

        internal string ConnectionString { get => configuration[AzureStorageAccount]; }

        internal async Task<BlobContainerClient> GetContainerClient(string containerName)
        {
            containerName = containerName.ToLower();
            if (BlobContainerClients.ContainsKey(containerName))
            {
                try
                {
                    return BlobContainerClients[containerName];
                }
                catch (Exception e)
                {
                    logger.LogError(e, "Error Occured Retrieving Client for {containerName}." + e.Message);
                    throw e;
                }
            }
            else
            {
                try
                {
                    var containerClient = new BlobContainerClient(configuration[AzureStorageAccount], containerName);
                    BlobContainerClients[containerName] = containerClient;
                    await containerClient.CreateIfNotExistsAsync();
                    return containerClient;
                }
                catch (Exception e)
                {
                    logger.LogError(e, "Error Occured Creating Container Client for {containerName}." + e.Message);
                    throw e;
                }
            }

        }
    }
}
