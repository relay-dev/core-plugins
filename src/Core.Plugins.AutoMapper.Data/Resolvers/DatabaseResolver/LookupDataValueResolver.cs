//using Core.Caching;
//using Core.Data;
//using Core.Plugins.AutoMapper.Data.LookupData;
//using Core.Plugins.AutoMapper.Data.Resolvers.Base;
//using System;
//using System.Collections.Generic;
//using System.Data;
//using System.Linq;

//namespace Core.Plugins.AutoMapper.Data.Resolvers.DatabaseResolver
//{
//    public class LookupDataValueResolver<T> : LookupDataValueResolverBase<T>
//    {
//        private readonly IDatabaseFactory _databaseFactory;

//        public LookupDataValueResolver(IDatabaseFactory databaseFactory, ICacheHelper cacheHelper)
//            : base(cacheHelper)
//        {
//            _databaseFactory = databaseFactory;
//        }
        
//        protected override Dictionary<T, string> GetDictionaryToCache(LookupDataByKey<T> lookupDataByKey)
//        {
//            DataTable dataTable = _databaseFactory.Create(lookupDataByKey.DataSource)
//                .Execute($"SELECT * FROM {lookupDataByKey.TableName}");

//            string columnNameOfPrimaryKey = lookupDataByKey.ColumnNameOfPrimaryKey ?? dataTable.Columns[0].ColumnName;
//            string columnNameOfField = lookupDataByKey.ColumnNameOfField ?? dataTable.Columns[1].ColumnName;

//            return dataTable.AsEnumerable()
//                .ToDictionary(dr => (T)dr[columnNameOfPrimaryKey], dr => dr[columnNameOfField] == DBNull.Value ? null : dr[columnNameOfField].ToString());
//        }
//    }
//}
