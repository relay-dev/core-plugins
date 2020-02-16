using Core.Data;
using Core.Framework.Attributes;
using Core.Framework.Descriptor;
using Core.Plugins.Extensions;
using Core.Providers;
using System;

namespace Core.Plugins.Data
{
    [Component(Type = Constants.Infrastructure.ComponentType.UnitOfWork, Name = Constants.Infrastructure.Component.AuditableDbContextUnitOfWork, PluginName = Constants.Infrastructure.Plugin.CoreDataAccess)]
    internal class AuditableDbContextUnitOfWork : DbContextUnitOfWork
    {
        private readonly IDbContext _dbContext;
        private readonly IUsernameProvider _usernameProvider;
        private readonly IDateTimeProvider _dateTimeProvider;

        public AuditableDbContextUnitOfWork(IDbContext dbContext, IUsernameProvider usernameProvider, IDateTimeProvider dateTimeProvider)
            : base(dbContext)
        {
            _dbContext = dbContext;
            _usernameProvider = usernameProvider;
            _dateTimeProvider = dateTimeProvider;
        }

        public override int Commit()
        {
            foreach (object entry in _dbContext.Tracker)
            {
                if (entry is IHaveAnID entryWithId)
                {
                    string username = _usernameProvider.Get().ThrowIfNullOrEmpty("IUsernameProvider");
                    DateTime serverDateTime = _dateTimeProvider.Get().ThrowIfNullOrEmpty("IDateTimeProvider");

                    if (entryWithId.ID < 1)
                    {
                        var entryWithCreatedFields = entry as IHaveCreatedFields;
                        if (entryWithCreatedFields != null)
                        {
                            entryWithCreatedFields.CreatedBy = username;
                            entryWithCreatedFields.CreatedDate = serverDateTime;
                        }
                    }
                    else
                    {
                        var entryWithModifiedFields = entry as IHaveModifiedFields;
                        if (entryWithModifiedFields != null)
                        {
                            entryWithModifiedFields.ModifiedBy = username;
                            entryWithModifiedFields.ModifiedDate = serverDateTime;
                        }
                    }
                }
            }

            return base.Commit();
        }
    }
}
