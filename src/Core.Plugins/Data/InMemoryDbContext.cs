﻿using Core.Caching;
using Core.Data;
using Core.Framework;
using Core.Providers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Transactions;
using static Core.Plugins.Constants.PluginConstants.Infrastructure;

namespace Core.Plugins.Data
{
    [Component(Type = ComponentType.DbContext, Name = Component.InMemoryDbContext, PluginName = Plugin.CoreDataAccess)]
    internal class InMemoryDbContext : IDbContext
    {
        private readonly ICacheHelper _cacheHelper;
        private readonly Lazy<ISequenceProvider> _sequenceProvider;
        private readonly object __lockObject = new object();

        public InMemoryDbContext(ICacheHelper cacheHelper, Lazy<ISequenceProvider> sequenceProvider)
        {
            _cacheHelper = cacheHelper;
            _sequenceProvider = sequenceProvider;
            Tracker = new List<object>();
        }

        public int SaveChanges()
        {
            foreach (object entry in Tracker)
            {
                var entryWithId = entry as IHaveAnId<long>;
                if (entryWithId != null)
                {
                    if (entryWithId.Id < 1)
                    {
                        entryWithId.Id = _sequenceProvider.Value.Get(entry.GetType().Name);
                    }
                }
            }

            using (var tx = new TransactionScope())
            {
                foreach (Type type in Tracker.Select(entity => entity.GetType()).Distinct())
                {
                    string cacheKey = _cacheHelper.FormatKey(nameof(type));

                    lock (__lockObject)
                    {
                        _cacheHelper.Remove(cacheKey);
                        _cacheHelper.GetOrSet(cacheKey, () => 
                        {
                            return Tracker.Where(entity => entity.GetType() == type).ToList();
                        });
                    }
                }

                tx.Complete();
            }
            
            return Tracker.Count;
        }

        public async Task<int> SaveChangesAsync()
        {
            return SaveChanges();
        }

        public ICollection<TEntity> Set<TEntity>() where TEntity : class
        {
            string cacheKey = _cacheHelper.FormatKey(nameof(TEntity));

            List<TEntity> entites = _cacheHelper.GetOrSet(cacheKey, () => new List<TEntity>()).ToList();

            Tracker.Add(entites);

            return entites;
        }

        public ICollection<object> Tracker { get; }

        private bool TryGetEntityId(object entry, out long id)
        {
            if (entry is IHaveAnId<int> entryWithIntId)
            {
                id = entryWithIntId.Id;
                return true;
            }

            if (entry is IHaveAnId<long> entryWithLongId)
            {
                id = entryWithLongId.Id;
                return true;
            }

            id = -1;
            return false;
        }
    }
}
