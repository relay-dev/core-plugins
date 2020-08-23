﻿using Core.Data;
using Core.Framework;
using System.Collections.Generic;
using System.Linq;
using static Core.Plugins.Constants.PluginConstants.Infrastructure;

namespace Core.Plugins.Data
{
    [Component(Type = ComponentType.Repository, Name = Component.DbContextRepository, PluginName = Plugin.CoreDataAccess)]
    internal class DbContextRepository<TEntity> : IRepository<TEntity> where TEntity : class
    {
        private readonly ICollection<TEntity> _dbSet;

        public DbContextRepository(IDbContext dbContext)
        {
            _dbSet = dbContext.Set<TEntity>();
        }

        public void Add(TEntity entity)
        {
            _dbSet.Add(entity);
        }

        public void Delete(TEntity entity)
        {
            _dbSet.Remove(entity);
        }

        public IEnumerable<TEntity> GetAll()
        {
            return _dbSet.AsEnumerable();
        }

        public IQueryable<TEntity> Query()
        {
            return _dbSet.AsQueryable();
        }
    }
}
