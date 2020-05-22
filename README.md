# MongoRepository
This is a repository with MongoDB. It implements the repository pattern.

## How to use it
You wil find the generic repository `GenericMongoRepository`. This is a `IGenericMongoRepository`as well, so you are able to register in your IoC container your own implementation of the refered interface.

In order to make your life easier, you can quickly implement any specific repository by inhereting from `GenericMongoRepository`. Example:
```C#
public class MongoRepositoryFake : GenericMongoRepository<TEntity>
{    
}
```
The `TEntity`is any class you want as your entity to save into the MongoDB. As well, you can pass to MongoDB the configuration:
- ConnectionString (:string) (optional) : It's the connection string to the MongoDB. If do not inform this parameter, MongoDB will use its default configuration (check the MongoDB documentation for more details).
- DataBase (:string) : It's the data base into the MongoDB you want to connecto to.
- CollectionDataBase (:string) : It's the collection where you want to store your documents.

Examples:
Without `ConnectionString`:
```C#
public class MongoRepositoryFake : GenericMongoRepository<AnyEntity>
{
    private const string DataBaseTests = "DataBaseTests";
    private const string UsersCollection = "CollectionTests";

    public MongoRepositoryFake() 
        : base(DataBaseTests, UsersCollection)
    {
    }
}
```
With `ConnectionString`:
```C#
public class MongoRepositoryFake : GenericMongoRepository<AnyEntity>
{
    private const string DataBaseTests = "DataBaseTests";
    private const string UsersCollection = "CollectionTests";
    private const string ConnectionString = "My connection string";

    public MongoRepositoryFake() 
        : base(ConnectionString, DataBaseTests, UsersCollection)
    {
    }
}
```

**Note:** Consider using the environment variables to provide this settings. This is only a template ;)

## Tests
This code has been tested by **Integration Tests**, so will need a MongoDB running on you local. I suggest get the Docker image. Following, these are the commands to pull, run and start the image:

Pulling the image:
```
docker pull mongo
```
Running for the first time:
```
docker run --name mongodb -v mongodata:/data/db -d -p 27017:27017 mongo
```
Starting your existing container (when it is stopped):
```
docker start mongodb
```
