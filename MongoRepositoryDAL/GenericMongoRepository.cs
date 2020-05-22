using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using MongoDB.Driver;

namespace MongoRepositoryDAL
{
    public class GenericMongoRepository<TEntity> : IGenericMongoRepository<TEntity> where TEntity : class
    {
        private IMongoCollection<TEntity> collection;
        private Action dropCollection;

        protected GenericMongoRepository(string dataBase, string collectionDataBase)
        {
            this.Setup(string.Empty, dataBase, collectionDataBase);
        }

        protected GenericMongoRepository(string connectionString, string dataBase, string collectionDataBase)
        {
            this.Setup(connectionString, dataBase, collectionDataBase);
        }

        private void Setup(string connectionString, string dataBase, string collectionDataBase)
        {
            if (string.IsNullOrEmpty(dataBase))
            {
                throw new ArgumentNullException($"The argument {nameof(dataBase)} was '{dataBase}'");
            }

            if (string.IsNullOrEmpty(collectionDataBase))
            {
                throw new ArgumentNullException($"The argument {nameof(collectionDataBase)} was '{collectionDataBase}'");
            }

            var client = string.IsNullOrEmpty(connectionString) ? new MongoClient() : new MongoClient(connectionString);

            var database = client.GetDatabase(dataBase);
            this.dropCollection = () => { database.DropCollection(collectionDataBase); };
            this.collection = database.GetCollection<TEntity>(collectionDataBase);
        }

        public void Save(TEntity entity)
        {
            this.collection.InsertOne(entity);
        }

        public void Save(IEnumerable<TEntity> entities)
        {
            this.collection.InsertMany(entities);
        }

        public IEnumerable<TEntity> Get(Expression<Func<TEntity, bool>> predicate)
        {
            var filter = Builders<TEntity>.Filter.Where(predicate);
            return this.collection.Find(filter).ToEnumerable();
        }

        public IEnumerable<TEntity> Get(Expression<Func<TEntity, bool>> predicate, int limit)
        {
            var filter = Builders<TEntity>.Filter.Where(predicate);
            return this.collection.Find(filter).Limit(limit).ToEnumerable();
        }

        public void Update<TUEntity>(
            Expression<Func<TEntity, bool>> predicate, 
            Expression<Func<TEntity, TUEntity>> updateField, TUEntity newValue) 
            where TUEntity : class
        {
            var filter = Builders<TEntity>.Filter.Where(predicate);
            var updateDefinition = Builders<TEntity>.Update.Set(updateField, newValue);

            this.collection.UpdateMany(filter, updateDefinition, new UpdateOptions { IsUpsert = false });
        }

        public void Delete(Expression<Func<TEntity, bool>> predicate)
        {
            var filter = Builders<TEntity>.Filter.Where(predicate);
            this.collection.DeleteMany(filter);
        }

        public void DropCollection()
        {
            this.dropCollection();
        }
    }
}
