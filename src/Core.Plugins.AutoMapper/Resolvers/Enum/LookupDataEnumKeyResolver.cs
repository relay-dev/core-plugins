using AutoMapper;
using Core.Caching;
using Core.Exceptions;
using Core.Plugins.AutoMapper.LookupData;
using Core.Providers;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace Core.Plugins.AutoMapper.Resolvers.Enum
{
    public class LookupDataEnumKeyResolver<T> : LookupDataResolverBase, IMemberValueResolver<object, object, System.Enum, T>
    {
        private readonly ICache _cache;

        public LookupDataEnumKeyResolver(IConnectionStringProvider connectionStringProvider, ICache cache)
            : base(connectionStringProvider)
        {
            _cache = cache;
        }

        public T Resolve(object source, object destination, System.Enum sourceMember, T destMember, ResolutionContext context)
        {
            if (sourceMember.ToString().ToLower() == "undefined")
            {
                return default;
            }

            LookupDataEnumAttribute lookupDataEnumAttribute = sourceMember.GetType()
                .GetCustomAttributes(typeof(LookupDataEnumAttribute), true)
                .Cast<LookupDataEnumAttribute>()
                .FirstOrDefault();

            if (lookupDataEnumAttribute == null)
            {
                throw new CoreException($"An attempt was made to Convert the Enum {sourceMember.GetType().Name} to a Primary Key without the [LookupDataEnum] attribute. Please add this attribute to the Enum, and also note that you can specify the Table Name to reference, as well as the Column Name of the Primary Key and the Column Name that contains the field to lookup. You do not have to provide these fields if your Enum follows standard conventions.");
            }

            string tableName = lookupDataEnumAttribute.TableName ?? sourceMember.GetType().Name;
            string cacheKey = _cache.FormatKey(CacheKeyPrefix, tableName);

            T result = GetPrimaryKeyForLookupValueFromCache(sourceMember, lookupDataEnumAttribute, tableName, cacheKey);

            // SF: This is a type of self-healing mechanism whereby if we can't find the value the first time, we'll clear the cache for that table only and make a fresh trip in case we are out of sync. If we miss twice, we won't try again
            if (EqualityComparer<T>.Default.Equals(result, default))
            {
                _cache.Remove(cacheKey);

                result = GetPrimaryKeyForLookupValueFromCache(sourceMember, lookupDataEnumAttribute, tableName, cacheKey);
            }

            return result;
        }

        private T GetPrimaryKeyForLookupValueFromCache(System.Enum source, LookupDataEnumAttribute lookupDataEnumAttribute, string tableName, string cacheKey)
        {
            DataTable dataTable =
                _cache.GetOrSet(cacheKey, () =>
                {
                    try
                    {
                        string sql = $"SELECT * FROM {tableName}";

                        return ExecuteSql(sql, lookupDataEnumAttribute.DataSource);
                    }
                    catch (Exception e)
                    {
                        if (!e.Message.StartsWith("Invalid object name"))
                            throw;

                        throw new CoreException(e, $"Attempted to map an Enum to a {typeof(T).Name} using ResolveUsing<LookupDataProvider<T>>().FromMember() but the defaults did not work. This is usually because the name of your Enum does not follow the convention of tblEnumName (you tried to query the lookup table {tableName}, which likly doesn't exist). If so, you must add an Attribute to your Enum Type like so: [LookupDataEnumAttribute(TableName = <<Name of lookup data table your enum corresponds with>>)]. You can also specify the column name for which to match the name with, if the column is not in the second position");
                    }

                }, DefaultCacheTimeoutInHours);

            string columnNameOfPrimaryKey = lookupDataEnumAttribute.ColumnNameOfPrimaryKey ?? dataTable.Columns[0].ColumnName;
            string columnNameOfFieldName = lookupDataEnumAttribute.ColumnNameOfFieldName ?? dataTable.Columns[1].ColumnName;

            DataRow dataRow = dataTable
                .AsEnumerable()
                .FirstOrDefault(dr => dr[columnNameOfFieldName] != DBNull.Value && dr[columnNameOfFieldName].ToString().Replace(" ", string.Empty).ToLower() == source.ToString().ToLower());

            if (dataRow == null)
            {
                return default;
            }

            return (T)dataRow[columnNameOfPrimaryKey];
        }
    }
}
