using Azure;
using Azure.Core.Serialization;
using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using Azure.Storage.Blobs.Specialized;
using Microsoft.Extensions.Logging;
using SimpleAzBlob.Clients;
using SimpleAzBlob.Interface;
using SimpleAzBlob.Models.Enums;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace SimpleAzBlob.Storage.BlobStorage
{
    public class SimpleAzBlobClient<AccountType> : ISimpleAzBlobClient<AccountType>
    {
        private readonly ILogger<SimpleAzBlobClient<AccountType>> logger;
        private readonly SimpleAzBlobContainerManager<AccountType> simpleAzBlobContainerManager;

        public SimpleAzBlobClient(ILogger<SimpleAzBlobClient<AccountType>> logger, SimpleAzBlobContainerManager<AccountType> simpleAzBlobContainerManager)
        {
            this.logger = logger;
            this.simpleAzBlobContainerManager = simpleAzBlobContainerManager;
        }

        public async Task SaveBlob(string containerName, string absoluteName, object item)
        {
            var containerClient = await simpleAzBlobContainerManager.GetContainerClient(containerName);
            using (var stream = (await JsonObjectSerializer.Default.SerializeAsync(item)).ToStream())
                await UploadBlob(absoluteName, stream, containerClient);

        }

        public async Task SaveBlob(string containerName, string absoluteName, Stream item)
        {
            var containerClient = await simpleAzBlobContainerManager.GetContainerClient(containerName);
            await UploadBlob(absoluteName, item, containerClient);
        }

        public async Task SaveBlob(string containerName, string path, string itemName, object item)
        {
            var containerClient = await simpleAzBlobContainerManager.GetContainerClient(containerName);
            using (var stream = (await JsonObjectSerializer.Default.SerializeAsync(item)).ToStream())
                await UploadBlob(Path.Combine(path, itemName), stream, containerClient);
        }

        public async Task SaveBlob(string containerName, string path, string itemName, Stream item)
        {
            var containerClient = await simpleAzBlobContainerManager.GetContainerClient(containerName);
            await UploadBlob(Path.Combine(path, itemName), item, containerClient);
        }

        private async Task UploadBlob(string path, Stream stream, BlobContainerClient containerClient)
        {
            try
            {

                var blobClient = new BlockBlobClient(simpleAzBlobContainerManager.ConnectionString, containerClient.Name, path);
                using (var itemSteam = stream)
                    await blobClient.UploadAsync(itemSteam, new BlobUploadOptions());

            }
            catch (Exception ex)
            {
                logger.LogError(ex, $"Error Uploading Blob {path}. " + ex.Message);
                throw ex;
            }
        }

        public async Task<T> GetBlob<T>(string containerName, string absoluteBlobName)
        {
            var containerClient = await simpleAzBlobContainerManager.GetContainerClient(containerName);
            return (await DownloadBlob(containerClient, absoluteBlobName))
            .ToObject<T>(JsonObjectSerializer.Default);
        }

        public async Task<Stream> GetBlob(string containerName, string absoluteBlobName)
        {
            var containerClient = await simpleAzBlobContainerManager.GetContainerClient(containerName);
            return (await DownloadBlob(containerClient, absoluteBlobName)).ToStream();
        }

        public async Task<T> GetBlob<T>(string containerName, string path, string itemName)
        {
            var containerClient = await simpleAzBlobContainerManager.GetContainerClient(containerName);
            return (await DownloadBlob(containerClient, Path.Combine(path, itemName)))
                .ToObject<T>(JsonObjectSerializer.Default);
        }

        public async Task<Stream> GetBlob(string containerName, string path, string itemName)
        {
            var containerClient = await simpleAzBlobContainerManager.GetContainerClient(containerName);
            return (await DownloadBlob(containerClient, Path.Combine(path, itemName))).ToStream();
        }


        private async Task<BinaryData> DownloadBlob(BlobContainerClient containerClient, string path)
        {
            var blobClient = containerClient.GetBlobClient(path);
            var result = await blobClient.DownloadContentAsync();
            return result.Value.Content;
        }

        public async Task<List<string>> ListAllBlobs(string containerName, string prefix = null)
        {
            var stack = new ConcurrentStack<string>();
            var containerClient = await simpleAzBlobContainerManager.GetContainerClient(containerName);

            var resultSegment = containerClient.GetBlobsAsync(prefix: prefix)
                .AsPages();

            await foreach (Page<BlobItem> blobPage in resultSegment)
            {
                if (blobPage.Values.Count > 0)
                    stack.PushRange(blobPage.Values.Select(s => s.Name).ToArray());
            }
            return stack.ToList();
        }

        public async Task<List<string>> ListFoldersAtPath(string containerName, string path)
        {
            return await ListItemsInSpecifiedFolder(containerName, ContainerItemType.Folder, path);
        }


        public async Task<List<string>> ListBlobsAtPath(string containerName, string path)
        {
            return await ListItemsInSpecifiedFolder(containerName, ContainerItemType.Blob, path);
        }

        private async Task<List<string>> ListItemsInSpecifiedFolder(string containerName, ContainerItemType containerItemType, string prefix = null)
        {
            var stack = new ConcurrentStack<string>();
            var containerClient = await simpleAzBlobContainerManager.GetContainerClient(containerName);
            try
            {
                // Call the listing operation and return pages of the specified size.
                var resultSegment = containerClient.GetBlobsByHierarchyAsync(prefix: prefix, delimiter: "/")
                    .AsPages();

                await foreach (Page<BlobHierarchyItem> blobPage in resultSegment)
                {
                    switch (containerItemType)
                    {
                        case ContainerItemType.Blob:
                            var blobs = blobPage.Values.Where(w => w.IsBlob).Select(s => s.Blob.Name).ToArray();
                            if (blobs.Length > 0)
                                stack.PushRange(blobs);
                            break;
                        case ContainerItemType.Folder:
                            var folders = blobPage.Values.Where(w => w.IsPrefix).Select(s => s.Prefix).ToArray();
                            if (folders.Length > 0)
                                stack.PushRange(folders);
                            break;
                    }
                }
                return stack.ToList();
            }
            catch (Exception ex)
            {
                logger.LogError(ex, $"Error Getting List of Items in {containerName}/{prefix ?? ""}. " + ex.Message);
                throw ex;
            }


        }

        public async Task DeleteBlob(string containerName, string absoluteBlobName)
        {
            var client = await simpleAzBlobContainerManager.GetContainerClient(containerName);
            await client.DeleteBlobIfExistsAsync(absoluteBlobName);
        }

        public async Task DeleteBlob(string containerName, string path, string blobName)
        {
            var client = await simpleAzBlobContainerManager.GetContainerClient(containerName);
            await client.DeleteBlobIfExistsAsync(Path.Combine(path, blobName));
        }
    }
}