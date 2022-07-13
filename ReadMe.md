# Simple AzBlob

A wrapper library around Azure Blob Storage SDK for simple implementation.

## Prerequisites
- IConfiguration
- ILogger
- "AzureStorageAccount" key set in app configuration.

## Setup
Use the dependancy injection extension to initialize logging.


	var configuration = new ConfigurationBuilder()
					.AddJsonFile("./appSettings.json")
					.Build();SimpleAzBlobClient

	serviceCollection.AddLogging();
	serviceCollection.AddSingleton<IConfiguration>(configuration);
	serviceCollection.AddSimpleAzBlob();

You can also use generics when using multiple storage accounts. 
	var configuration = new ConfigurationBuilder()
					.AddJsonFile("./appSettings.json")
					.Build();SimpleAzBlobClient

	serviceCollection.AddLogging();
	serviceCollection.AddSingleton<IConfiguration>(configuration);
	serviceCollection.AddSimpleAzBlob<StorageAccountTypeA>();
	serviceCollection.AddSimpleAzBlob<StorageAccountTypeB>();

## Usage
When passing a container name, the container will be created if it doesn't exist.



	public class FooClass
	{
		private readonly ISimpleAzBlobClient simpleAzBlobClient;
		private readonly ISimpleAzBlobClient<StorageAccountTypeA> simpleAzBlobClientAccountA;
		private readonly ISimpleAzBlobClient<StorageAccountTypeB> simpleAzBlobClientAccountB;
		privtae readonly string containerName = "doodleContainer";

		public FooClass(ISimpleAzBlobClient simpleAzBlobClient, ISimpleAzBlobClient<StorageAccountTypeA> simpleAzBlobClientAccountA, ISimpleAzBlobClient<StorageAccountTypeB> simpleAzBlobClientAccountB)
		{
			this.simpleAzBlobClient = simpleAzBlobClient;
			this.simpleAzBlobClientAccountA = simpleAzBlobClientAccountA;
			this.simpleAzBlobClientAccountB = simpleAzBlobClientAccountB;
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

