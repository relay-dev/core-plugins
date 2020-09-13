using Core.Framework;
using Core.Providers;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using System;
using System.Collections.Generic;

namespace Core.Plugins.EntityFramework.Auditor.Impl
{
    public class EntityFrameworkEntityAuditor : IEntityAuditor
    {
        private readonly IUsernameProvider _usernameProvider;
        private readonly IDateTimeProvider _dateTimeProvider;

        public EntityFrameworkEntityAuditor(
            IUsernameProvider usernameProvider,
            IDateTimeProvider dateTimeProvider)
        {
            _usernameProvider = usernameProvider;
            _dateTimeProvider = dateTimeProvider;
        }

        public void Audit(IEnumerable<EntityEntry> entities)
        {
            foreach (EntityEntry entry in entities)
            {
                string username = _usernameProvider.Get();
                DateTime serverDateTime = _dateTimeProvider.Get();

                switch (entry.State)
                {
                    case EntityState.Added:
                        if (entry.Entity is IHaveCreatedFields createdEntity)
                        {
                            createdEntity.CreatedBy = username;
                            createdEntity.CreatedDate = serverDateTime;
                        }
                        break;
                    case EntityState.Modified:
                        if (entry.Entity is IHaveModifiedFields modifiedEntity)
                        {
                            modifiedEntity.ModifiedBy = username;
                            modifiedEntity.ModifiedDate = serverDateTime;
                        }
                        break;
                }
            }
        }
    }
}
