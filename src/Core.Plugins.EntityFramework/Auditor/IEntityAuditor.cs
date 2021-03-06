﻿using Microsoft.EntityFrameworkCore.ChangeTracking;
using System.Collections.Generic;

namespace Core.Plugins.EntityFramework.Auditor
{
    public interface IEntityAuditor
    {
        void Audit(IEnumerable<EntityEntry> entities);
    }
}