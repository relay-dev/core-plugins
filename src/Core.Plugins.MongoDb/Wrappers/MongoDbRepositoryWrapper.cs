using Core.Data;
using Core.Exceptions;
using Core.Framework;
using Core.Providers;
using MongoDB.Driver;
using System.Collections.Generic;
using System.Linq;

namespace Core.Plugins.MongoDb.Wrappers
{
    public class MongoDbRepositoryWrapper<TEntity> : IRepository<TEntity> where TEntity : class
    {
        private readonly IMongoDatabase _mongoDb;
        private readonly IPageProvider<TEntity> _pageProvider;

        public MongoDbRepositoryWrapper(
            MongoClient mongoClient,
            IPageProvider<TEntity> pageProvider)
        {
            _mongoDb = mongoClient.GetDatabase(typeof(TEntity).Name);
            _pageProvider = pageProvider;
        }

        public IQueryable<TEntity> Query()
        {
            return _mongoDb.GetCollection<TEntity>(typeof(TEntity).Name).AsQueryable();
        }

        public IPageable<TEntity> Page()
        {
            return _pageProvider.Get();
        }

        public IEnumerable<TEntity> GetAll()
        {
            return _mongoDb.GetCollection<TEntity>(typeof(TEntity).Name).AsQueryable().AsEnumerable();
        }

        public void Add(TEntity entity)
        {
            _mongoDb.GetCollection<TEntity>(typeof(TEntity).Name).InsertOne(entity);
        }

        public void Delete(TEntity entity)
        {
            var entityWithId = entity as IHaveAnId<long>;
            if (entityWithId == null)
            {
                throw new CoreException("The entity parameter must implement IHaveAnId in order to use the Delete() method when using the MongoDbRepository");
            }

            _mongoDb.GetCollection<TEntity>(typeof(TEntity).Name).DeleteOne(Builders<TEntity>.Filter.Eq("ID", entityWithId.Id));
        }

        public void Delete(ICollection<TEntity> entities)
        {
            entities.ToList().ForEach(Delete);
        }
    }
}
