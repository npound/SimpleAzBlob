# Simple Az Blob

A wrapper library around Azure Blob Storage SDK for simple implementation.

## Prerequisites
- IConfiguration
- ILogger
- "AzureStorageAccount" key set in app configuration.

## Setup
Use the dependancy injection extension to initialize logging

ISimpleAzBlobClient
	var configuration = new ConfigurationBuilder()
					.AddJsonFile("./appSettings.json")
					.Build();SimpleAzBlobClient

	serviceCollection.AddLogging();
	serviceCollection.AddSingleton<IConfiguration>(configuration);
	serviceCollection.AddBlobStorage();



## Usage
When passing a container name, the container will be created if it doesn't exist.



	public class FooClass
	{
		private readonly ISimpleAzBlobClient simpleAzBlobClient;
		privtae readonly string containerName = "doodleContainer";

		public FooClass(ISimpleAzBlobClient simpleAzBlobClient)
		{
			this.simpleAzBlobClient = simpleAzBlobClient;
		}

		public async Task Test()
		{
			var item = new Foo {
				foo = "a",
				bar = "b"
			}
			var list = await simpleAzBlobClient.ListBlobsAtPath(containerName);

			//This will save it at root of container
			await simpleAzBlobClient.SaveBlob(containerName,"fooBlob", item);

			var sameItem = await simpleAzBlobClient.GetBlob<Foo>(containerName,"fooBlob");
			await simpleAzBlobClient.DeleteBlob(containerName, "fooBlob");
		}
	}

