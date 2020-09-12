namespace Core.Plugins.Framework
{
    /// <summary>
    /// Constant strings that are referenced project-wide, and potentially by consumers
    /// </summary>
    public static class PluginConstants
    {
        /// <summary>
        /// Constant strings related to Infrastructure
        /// </summary>
        public static class Infrastructure
        {
            /// <summary>
            /// Constant strings related to the IoC Container frameworks
            /// </summary>
            public static class IoCContainer
            {
                /// <summary>
                /// CastleWindsor is an IoC and AOP application framework
                /// </summary>
                public const string CastleWindsor = "CastleWindsor";
            }

            /// <summary>
            /// Constant strings related to the Plugins supported by the Core framework
            /// </summary>
            public static class Plugin
            {
                /// <summary>
                /// AutoMapper is a mapping product that supports re-usability and automates as much as possible when it comes to mapping types
                /// </summary>
                public const string AutoMapper = "AutoMapper";

                /// <summary>
                /// AutoMapperDataAccess is a plugin that integrates the Core.DataAccess features with AutoMapper features, specifically the LookupDataResolvers
                /// </summary>
                public const string AutoMapperData = "AutoMapperData";

                /// <summary>
                /// Memory Cache is a caching plugin that supports reading reading and writing to a cache by way of the ICache interface
                /// </summary>
                public const string MemoryCache = "MemoryCache";

                /// <summary>
                /// Azure Configuration is a plugin that supports reading Azure application configuration files by way of the IConfiguration interface
                /// </summary>
                public const string AzureConfiguration = "AzureConfiguration";

                /// <summary>
                /// Core Data Access is an API layer over ADO.NET
                /// </summary>
                public const string CoreDataAccess = "CoreDataAccess";

                /// <summary>
                /// Core Providers are utility classes
                /// </summary>
                public const string CoreProviders = "CoreProviders";

                /// <summary>
                /// Entity Framework is Microsoft's current flavor of ORM
                /// </summary>
                public const string EntityFramework = "EntityFramework";

                /// <summary>
                /// Fluent Validation is an object validation library with a focus on readable and reusable coded rule sets
                /// </summary>
                public const string FluentValidation = "FluentValidation";

                /// <summary>
                /// Log4Net is an open-source logging framework
                /// </summary>
                public const string Log4Net = "Log4Net";

                /// <summary>
                /// SQL Server database
                /// </summary>
                public const string SqlServer = "SqlServer";

                /// <summary>
                /// MongoDB is a non-relational, document database
                /// </summary>
                public const string MongoDb = "MongoDb";
            }

            /// <summary>
            /// Constant strings related to the Plugin Types supported by the Core framework
            /// </summary>
            public static class PluginType
            {
                /// <summary>
                /// A Plugin that maps types
                /// </summary>
                public const string Mapper = "Mapper";

                /// <summary>
                /// A Plugin that maps types and accesses a database directly within the mapper
                /// </summary>
                public const string MapperData = "MapperData";

                /// <summary>
                /// A Plugin that handles application caching
                /// </summary>
                public const string Caching = "Caching";

                /// <summary>
                /// A Plugin that handles application configuration
                /// </summary>
                public const string Configuration = "Configuration";

                /// <summary>
                /// A Plugin that supports a specific database type
                /// </summary>
                public const string Database = "Database";

                /// <summary>
                /// A Plugin that facilitates data access
                /// </summary>
                public const string DataAccess = "DataAccess";

                /// <summary>
                /// A Plugin that facilitates providers
                /// </summary>
                public const string Providers = "Providers";

                /// <summary>
                /// A Plugin that facilitates logging
                /// </summary>
                public const string Logger = "Logger";

                /// <summary>
                /// An Object Relational Mapping
                /// </summary>
                public const string Orm = "Orm";

                /// <summary>
                /// A Plugin that facilitates validation
                /// </summary>
                public const string Validation = "Validation";
            }

            /// <summary>
            /// Constant strings related to the Component options supported by the Core framework
            /// </summary>
            public static class Component
            {
                /// <summary>
                /// Repository - Entity Framework
                /// </summary>
                public const string EntityFrameworkRepository = "EntityFrameworkRepository";

                /// <summary>
                /// Repository - Entity Framework Pageable
                /// </summary>
                public const string EntityFrameworkPageableRepository = "EntityFrameworkPageableRepository";

                /// <summary>
                /// DbContext - Repository
                /// </summary>
                public const string DbContextRepository = "DbContextRepository";

                /// <summary>
                /// UnitOfWork - DbContext
                /// </summary>
                public const string DbContextUnitOfWork = "DbContextUnitOfWork";

                /// <summary>
                /// UnitOfWork - Auditable DbContext
                /// </summary>
                public const string AuditableDbContextUnitOfWork = "AuditableDbContextUnitOfWork";

                /// <summary>
                /// DbContext - In Memory
                /// </summary>
                public const string InMemoryDbContext = "InMemoryDbContext";

                /// <summary>
                /// Provider - DateTime Provider
                /// </summary>
                public const string DateTimeProvider = "DateTimeProvider";

                /// <summary>
                /// Provider - DateTime Utc Provider
                /// </summary>
                public const string DateTimeUtcProvider = "DateTimeUtcProvider";
            }

            /// <summary>
            /// Constant strings related to the Component Type options supported by the Core framework
            /// </summary>
            public static class ComponentType
            {
                /// <summary>
                /// Data Access - Repository
                /// </summary>
                public const string Repository = "Repository";

                /// <summary>
                /// Data Access - UnitOfWork
                /// </summary>
                public const string UnitOfWork = "UnitOfWork";

                /// <summary>
                /// Data Access - DbContext
                /// </summary>
                public const string DbContext = "DbContext";

                /// <summary>
                /// Provider - DateTime Provider
                /// </summary>
                public const string DateTimeProvider = "DateTimeProvider";
            }
        }
    }
}
