using Azure.Storage.Blobs;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using SimpleAzBlob.Models;
using System;
using System.Collections.Concurrent;
using System.Threading.Tasks;

namespace SimpleAzBlob.Clients
{
    public class SimpleAzBlobContainerManager<T>
    {
        private const string connectionString = "AzureStorageAccount";
        private readonly IConfiguration configuration;
        private readonly ILogger<SimpleAzBlobContainerManager<T>> logger;
        private readonly Models.ConnectionContainer<T> connectionContainer;
        private static ConcurrentDictionary<string, BlobContainerClient> BlobContainerClients = new ConcurrentDictionary<string, BlobContainerClient>();


        public SimpleAzBlobContainerManager(IConfiguration configuration, ILogger<SimpleAzBlobContainerManager<T>> logger, ConnectionContainer<T> connectionContainer)
        {
            this.configuration = configuration;
            this.logger = logger;
            this.connectionContainer = connectionContainer;
        }

        public SimpleAzBlobContainerManager(string connectionString, ILogger<SimpleAzBlobContainerManager<T>> logger = null)
        {
            this.logger = logger;
            connectionContainer = new ConnectionContainer<T>
            {
                ConnectionString = connectionString
            };
        }


        internal string ConnectionString { get => connectionContainer?.ConnectionString ?? configuration[connectionString]; }

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
                    if (logger != null)
                        logger.LogError(e, $"Error Occured Retrieving Client for {containerName}." + e.Message);
                    throw e;
                }
            }
            else
            {
                try
                {
                    var containerClient = new BlobContainerClient(ConnectionString, containerName);
                    BlobContainerClients[containerName] = containerClient;
                    await containerClient.CreateIfNotExistsAsync();
                    return containerClient;
                }
                catch (Exception e)
                {
                    if (logger != null)
                        logger.LogError(e, $"Error Occured Creating Container Client for {containerName}." + e.Message);
                    throw e;
                }
            }

        }
    }
}
