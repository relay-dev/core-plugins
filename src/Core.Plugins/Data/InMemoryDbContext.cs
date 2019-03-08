using Core.Caching;
using Core.Data;
using Core.Framework.Attributes;
using Core.Framework.Descriptor;
using Core.Providers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Transactions;

namespace Core.Plugins.Data
{
    [Component(Type = Constants.Infrastructure.ComponentType.DbContext, Name = Constants.Infrastructure.Component.InMemoryDbContext, PluginName = Constants.Infrastructure.Plugin.CoreDataAccess)]
    internal class InMemoryDbContext : IDbContext
    {
        private readonly ICacheFactory _cacheFactory;
        private readonly Lazy<ISequenceProvider> _sequenceProvider;
        private readonly object __lockObject = new object();

        public InMemoryDbContext(ICacheFactory cacheFactory, Lazy<ISequenceProvider> sequenceProvider)
        {
            _cacheFactory = cacheFactory;
            _sequenceProvider = sequenceProvider;
            Tracker = new List<object>();
        }

        public int SaveChanges()
        {
            foreach (object entry in Tracker)
            {
                var entryWithId = entry as IHaveAnID;
                if (entryWithId != null)
                {
                    if (entryWithId.ID < 1)
                    {
                        entryWithId.ID = _sequenceProvider.Value.Get(entry.GetType().Name);
                    }
                }
            }

            ICache cache = _cacheFactory.Create(GetType().Name);

            using (var tx = new TransactionScope())
            {
                foreach (Type type in Tracker.Select(entity => entity.GetType()).Distinct())
                {
                    string cacheKey = cache.FormatKey(nameof(type));

                    lock (__lockObject)
                    {
                        cache.Remove(cacheKey);
                        cache.GetOrAdd(cacheKey, () => 
                        {
                            return Tracker.Where(entity => entity.GetType() == type).ToList();
                        });
                    }
                }

                tx.Complete();
            }
            
            return Tracker.Count;
        }

        public ICollection<TEntity> Set<TEntity>() where TEntity : class
        {
            ICache cache = _cacheFactory.Create(GetType().Name);
            string cacheKey = cache.FormatKey(nameof(TEntity));

            List<TEntity> entites = cache.GetOrAdd(cacheKey, () => new List<TEntity>()).ToList();

            Tracker.Add(entites);

            return entites;
        }

        public ICollection<object> Tracker { get; }
    }
}
