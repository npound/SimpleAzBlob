using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SimpleAzBlob.Extensions;
using SimpleAzBlob.Interface;
using SimpleAzBlob.Storage.BlobStorage;
using System.Security.Authentication.ExtendedProtection;
namespace SimpleAzBlob.Test
{
    public class SimpleAzBlobTest
    {
        [Fact]
        public async Task TestDi()
        {

            var service = new ServiceCollection();

            service.AddLogging();
            service.AddSingleton<IConfiguration>(new ConfigurationBuilder()
                .AddJsonFile("appsettings.json")
                .Build());
            service.AddSimpleAzBlob();

            var provider = service.BuildServiceProvider();

            var client = provider.GetService<ISimpleAzBlobClient>();

            Assert.NotNull(client);

            var blobsAtPath = await client.ListBlobsAtPath("test", "foo");

            Assert.NotEmpty(blobsAtPath);

            var fileName = $"foo/anotherBlob-{Guid.NewGuid()}.json";

            await client.SaveBlob("test", fileName, new
            {
                cool = true
            });

            var blob = await client.GetBlob("test", fileName);

            Assert.NotNull(blob);

            await client.DeleteBlob("test", fileName);


        }

        [Fact]
        public async Task TestStandaloneClient()
        {
            var service = new ServiceCollection();
            service.AddSingleton<IConfiguration>(new ConfigurationBuilder()
                .AddJsonFile("appsettings.json")
                .Build());
            var provider = service.BuildServiceProvider();

            var config = provider.GetService<IConfiguration>();

            var client = new SimpleAzBlobClient(config["AzureStorageAccount"]);

            Assert.NotNull(client);


            var blobsAtPath = await client.ListBlobsAtPath("test","foo");

            Assert.NotEmpty(blobsAtPath);

            var fileName = $"foo/anotherBlob-{Guid.NewGuid()}.json";

            await client.SaveBlob("test", fileName, new
            {
                cool = true
            });

            var blob = await client.GetBlob("test", fileName);

            Assert.NotNull(blob);

            await client.DeleteBlob("test", fileName);

        }



    }



  
}