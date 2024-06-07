using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace SimpleAzBlob.Interface
{
    public interface ISimpleAzBlobClient<AccountType>
    {

        /// <summary>
        /// Gets an object from Blob Storage.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="containerName">Container the object lives in.</param>
        /// <param name="absoluteBlobName">Blob name that has folder path included in it.</param>
        /// <returns></returns>
        Task<T> GetBlob<T>(string containerName, string absoluteBlobName);
        /// <summary>
        /// Get file stream of given blob.
        /// </summary>
        /// <param name="containerName">Container the object lives in.</param>
        /// <param name="absoluteBlobName">Blob name that has folder path included in it.</param>
        /// <returns></returns>
        Task<Stream> GetBlob(string containerName, string absoluteBlobName);
        /// <summary>
        /// Gets an object from Blob Storage.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="containerName">Container the object lives in.</param>
        /// <param name="path">Path to folder.</param>
        /// /// <param name="itemName">Blob name that has folder path included in it.</param>
        /// <returns></returns>
        Task<T> GetBlob<T>(string containerName, string path, string itemName);
        /// <summary>
        /// Get file stream of given blob.
        /// </summary>
        /// <param name="containerName">Container the object lives in.</param>
        /// <param name="path">Path to folder.</param>
        /// /// <param name="itemName">Blob name that has folder path included in it.</param>
        /// <returns></returns>
        Task<Stream> GetBlob(string containerName, string path, string itemName);
        /// <summary>
        /// List all blob names in container or specified prefix if given. 
        /// </summary>
        /// <see cref="https://docs.microsoft.com/en-us/azure/storage/blobs/storage-blobs-list?tabs=dotnet"/>
        /// <param name="containerName">Name of the Container the blob lives in.</param>
        /// <param name="prefix">Specifies a string that filters the results to return only blobs whose absolute name(includes path) begins with the specified prefix.
        /// <example>For example: "FolderA/FolderB/"
        /// </example>
        /// </param>
        /// <returns>List of blob names including path.</returns>
        Task<List<string>> ListAllBlobs(string containerName, string prefix = null);

        /// <summary>
        /// List all blob names in the specified folder path given. 
        /// </summary>
        /// <see cref="https://docs.microsoft.com/en-us/azure/storage/blobs/storage-blobs-list?tabs=dotnet"/>
        /// <param name="containerName">Name of the container the blob lives in.</param>
        /// <param name="path">Path to the grab the list of blobs at. 
        /// <example>For example: "FolderA/FolderB/"
        /// </example>
        /// </param>
        /// <returns>List of blob names with path included.</returns>
        Task<List<string>> ListBlobsAtPath(string containerName, string path);

        /// <summary>
        /// List all folders at specified path. 
        /// </summary>
        /// <see cref="https://docs.microsoft.com/en-us/azure/storage/blobs/storage-blobs-list?tabs=dotnet"/>
        /// <param name="containerName">Name of the Container the folder lives in.</param>
        /// <param name="path">Path to the grab the list of folders at. 
        /// <example>For example: "FolderA/FolderB/"
        /// </example>
        /// </param>
        /// <returns>List of folder names with path included.</returns>
        Task<List<string>> ListFoldersAtPath(string containerName, string path);

        /// <summary>
        /// Saves Object to Blob Storage. Creates if doesn't exists, overwrites if does. 
        /// </summary>
        /// <param name="containerName">Container to save to.</param>
        /// <param name="absoluteName">Absolutely name including path.     
        /// <example>For example: "FolderA/FolderB/FooBlob"
        /// </example></param>
        /// <param name="item">Item Object To Save</param>
        /// <returns>Blob Deserialized to Object</returns>
        Task SaveBlob(string containerName, string absoluteName, object item);
        /// <summary>
        /// Saves stream to a given blob.
        /// </summary>
        /// <param name="containerName">Container to save to.</param>
        /// <param name="absoluteName">Absolutely name including path.     
        /// <example>For example: "FolderA/FolderB/FooBlob"
        /// </example></param>
        /// <param name="item">Steam of file to save.</param>
        /// <returns></returns>
        Task SaveBlob(string containerName, string absoluteName, Stream item);

        /// <summary>
        /// Saves Object to Blob Storage. Creates if doesn't exists, overwrites if does. 
        /// </summary>
        /// <param name="containerName">Container to save to.</param>
        /// <param name="path">Path of folder to save in.     
        /// <example>For example: "FolderA/FolderB"
        /// </example></param>
        /// <param name="itemName">Blob name.     
        /// <example>For example: "Foo Blob"
        /// </example></param>
        /// <param name="item">Item Object To Save</param>
        /// <returns></returns>
        Task SaveBlob(string containerName, string path, string itemName, object item);
        /// <summary>
        /// Saves stream to a given blob.
        /// </summary>
        /// <param name="containerName">Container to save to.</param>
        /// <param name="path">Path of folder to save in.     
        /// <example>For example: "FolderA/FolderB"
        /// </example></param>
        /// <param name="itemName">Blob name.     
        /// <example>For example: "Foo Blob"
        /// </example></param>
        /// <param name="item">Steam of file to save.</param>
        /// <returns></returns>
        Task SaveBlob(string containerName, string path, string itemName, Stream item);

        /// <summary>
        /// Deletes Blob
        /// </summary>
        /// <param name="containerName">Container the Blob lives in.</param>
        /// <param name="absoluteBlobName">Name of Blob Including Path</param>
        /// <returns></returns>
        Task DeleteBlob(string containerName, string absoluteBlobName);

        /// <summary>
        /// Deletes given blob at given path. 
        /// </summary>
        /// <param name="containerName">container blob lives in.</param>
        /// <param name="path">Folder path to blob.</param>
        /// <param name="blobName">Name of blob</param>
        /// <returns></returns>
        Task DeleteBlob(string containerName, string path, string blobName);
    }
}