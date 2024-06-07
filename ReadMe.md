# Simple AzBlob

A wrapper library around Azure Blob Storage SDK for simple implementation.

## Prerequisites
- IConfiguration
- ILogger
- "AzureStorageAccount" key set in app configuration.

### Local Development Prerequisites
- Azurite (Azure Storage Emulator was deprecated and incompatible)

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

    
    public class Foo
    {
        public string Moo { get; set; }
        public string Bar { get; set; }
    }

	public class FooClass
	{
		private readonly ISimpleAzBlobClient simpleAzBlobClient;
		private readonly string containerName = "doodleContainer";

		public FooClass(ISimpleAzBlobClient simpleAzBlobClient)
		{
			this.simpleAzBlobClient = simpleAzBlobClient;
		}

        public async Task Test()
        {
            var item = new Foo
            {
                Moo = "a",
                Bar = "b"
            };

            var list = await simpleAzBlobClient.ListBlobsAtPath(containerName);

            //This will save it at root of container
            await simpleAzBlobClient.SaveBlob(containerName, blobName, item);

            var sameItem = await simpleAzBlobClient.GetBlob<Foo>(containerName, blobName);
            await simpleAzBlobClient.DeleteBlob(containerName, blobName);
        }
	}
    

### Standalone Client
In cases a standalone client is needed. Pass the SimpleAzBlobClient a connection string as so. 

    
    public class Foo
    {
        public string Moo { get; set; }
        public string Bar { get; set; }
    }

    public class FooClass
    {
        private readonly ISimpleAzBlobClient simpleAzBlobClient;
        private readonly string containerName = "doodleContainer";
        private readonly string connectionString = "UseDevelopmentStorage=true";
        private readonly string blobName = "fooBlob";
        

        public FooClass()
        {
            this.simpleAzBlobClient = new SimpleAzBlobClient(connectionString);
        }

        public async Task Test()
        {
            var item = new Foo
            {
                Moo = "a",
                Bar = "b"
            };

            var list = await simpleAzBlobClient.ListBlobsAtPath(containerName);

            //This will save it at root of container
            await simpleAzBlobClient.SaveBlob(containerName, blobName, item);

            var sameItem = await simpleAzBlobClient.GetBlob<Foo>(containerName, blobName);
            await simpleAzBlobClient.DeleteBlob(containerName, blobName);
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
					.Build();

	var conn = "Connection String";
	serviceCollection.AddLogging();
	serviceCollection.AddSingleton<IConfiguration>(configuration);
	serviceCollection.AddSimpleAzBlob<AzureStorageAccountA>();
	serviceCollection.AddSimpleAzBlob<AzureStorageAccountB>(conn);
    

And just call it like so.

    
    public class Foo
    {
        public string Moo { get; set; }
        public string Bar { get; set; }
    }

	public class FooClass
	{
		private readonly ISimpleAzBlobClient<AzureStorageAccountA> simpleAzBlobClientAccountA;
		private readonly ISimpleAzBlobClient<AzureStorageAccountB> simpleAzBlobClientAccountB;
		private readonly string containerName = "doodleContainer";

		public FooClass(ISimpleAzBlobClient<AzureStorageAccountA> simpleAzBlobClientAccountA, ISimpleAzBlobClient<AzureStorageAccountB> simpleAzBlobClientAccountB)
		{
			this.simpleAzBlobClientAccountA = simpleAzBlobClientAccountA;
			this.simpleAzBlobClientAccountB = simpleAzBlobClientAccountB;
		}

        public async Task Test()
        {
            var item = new Foo
            {
                Moo = "a",
                Bar = "b"
            };

            var list = await simpleAzBlobClient.ListBlobsAtPath(containerName);

            //This will save it at root of container
            await simpleAzBlobClient.SaveBlob(containerName, blobName, item);

            var sameItem = await simpleAzBlobClient.GetBlob<Foo>(containerName, blobName);
            await simpleAzBlobClient.DeleteBlob(containerName, blobName);
        }

	}
    