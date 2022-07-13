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

## Multiple Account Handling
You can create generic classes labeled as configurations to pass in DI to facilitate multiple accounts like so.

   public class AzureStorageAccountA
    {
    }
	   public class AzureStorageAccountB
    {
    }

	var configuration = new ConfigurationBuilder()
					.AddJsonFile("./appSettings.json")
					.Build();SimpleAzBlobClient

	serviceCollection.AddLogging();
	serviceCollection.AddSingleton<IConfiguration>(configuration);
	serviceCollection.AddSimpleAzBlob<AzureStorageAccountA>();
	serviceCollection.AddSimpleAzBlob<AzureStorageAccountB>();


And just call it like so.

	public class FooClass
	{
		private readonly ISimpleAzBlobClient simpleAzBlobClient;
		private readonly ISimpleAzBlobClient<AzureStorageAccountA> simpleAzBlobClientAccountA;
		private readonly ISimpleAzBlobClient<AzureStorageAccountB> simpleAzBlobClientAccountB;
		privtae readonly string containerName = "doodleContainer";

		public FooClass(ISimpleAzBlobClient simpleAzBlobClient, ISimpleAzBlobClient<AzureStorageAccountA> simpleAzBlobClientAccountA, ISimpleAzBlobClient<AzureStorageAccountB> simpleAzBlobClientAccountB)
		{
			this.simpleAzBlobClient = simpleAzBlobClient;
			this.simpleAzBlobClientAccountA = simpleAzBlobClientAccountA;
			this.simpleAzBlobClientAccountB = simpleAzBlobClientAccountB;
		}

	}