using AutoMapper;
using Core.Caching;
using Core.Exceptions;
using Core.Plugins.AutoMapper.LookupData;
using Core.Plugins.Utilities;
using Core.Providers;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace Core.Plugins.AutoMapper.Resolvers.EnumResolvers
{
    public class LookupDataEnumValueResolver<TInput, TOutput> : LookupDataResolverBase, IMemberValueResolver<object, object, TInput, TOutput>
    {
        private readonly ICacheHelper _cacheHelper;

        public LookupDataEnumValueResolver(IConnectionStringProvider connectionStringProvider, ICacheHelper cacheHelper)
            : base(connectionStringProvider)
        {
            _cacheHelper = cacheHelper;

            DefaultTimeoutInHours = 24;
        }

        public int DefaultTimeoutInHours { get; set; }

        public TOutput Resolve(object source, object destination, TInput sourceMember, TOutput destMember, ResolutionContext context)
        {
            if (typeof(TInput) == typeof(string))
            {
                return GlobalHelper.ParseEnum<TOutput>(sourceMember as string);
            }

            LookupDataEnumAttribute lookupDataEnumAttribute = sourceMember.GetType()
                .GetCustomAttributes(typeof(LookupDataEnumAttribute), true)
                .Cast<LookupDataEnumAttribute>()
                .FirstOrDefault();

            if (lookupDataEnumAttribute == null)
            {
                throw new CoreException($"An attempt was made to Convert the value of {sourceMember} to Enum of type {typeof(TOutput).Name} without the [LookupDataEnum] attribute on the Enum. Please add this attribute to the Enum, and also note that you can specify the Table Name to reference, as well as the Column Name of the Primary Key and the Column Name that contains the field to lookup. You do not have to provide these fields if your Enum follows standard conventions.");
            }

            string tableName = lookupDataEnumAttribute.TableName ?? $"{typeof(TOutput).Name}";
            string cacheKey = _cacheHelper.FormatKey(CacheKeyPrefix, tableName);

            TOutput result = GetEnumFromIntForLookupValueFromCache(sourceMember, lookupDataEnumAttribute, lookupDataEnumAttribute.DataSource, tableName, cacheKey);

            // SF: This is a type of self-healing mechanism whereby if we can't find the value the first time, we'll clear the cache for that table only and make a fresh trip in case we are out of sync. If we miss twice, we won't try again
            if (EqualityComparer<TOutput>.Default.Equals(result, default(TOutput)))
            {
                _cacheHelper.Remove(cacheKey);

                result = GetEnumFromIntForLookupValueFromCache(sourceMember, lookupDataEnumAttribute, lookupDataEnumAttribute.DataSource, tableName, cacheKey);
            }

            return result;
        }

        private TOutput GetEnumFromIntForLookupValueFromCache(TInput source, LookupDataEnumAttribute lookupDataEnumAttribute, string dataSource, string tableName, string cacheKey)
        {
            DataTable dataTable =
                _cacheHelper.GetOrSet(cacheKey, () =>
                {
                    try
                    {
                        string sql = $"SELECT * FROM {tableName}";

                        return ExecuteSql(sql, dataSource);
                    }
                    catch (Exception e)
                    {
                        if (!e.Message.StartsWith("Invalid object name"))
                            throw;

                        throw new CoreException(e, $"Attempted to map the value of {source} to an Enum of type {typeof(TOutput).Name} using ResolveUsing<LookupDataProvider<T>>().FromMemeber() but the defaults did not work. This is usually because the name of your Enum does not follow the convention of tblEnumName (you tried to query the lookup table {tableName}, which likly doesn't exist). If so, you must add an Attribute to your Enum Type like so: [LookupDataEnumAttribute(TableName = <<Name of lookup data table your enum corresponds with>>)]. You can also specify the column name for which to match the name with, if the column is not in the second position");
                    }
                }, DefaultTimeoutInHours);

            string columnNameOfPrimaryKey = lookupDataEnumAttribute.ColumnNameOfPrimaryKey ?? dataTable.Columns[0].ColumnName;
            string columnNameOfFieldName = lookupDataEnumAttribute.ColumnNameOfFieldName ?? dataTable.Columns[1].ColumnName;

            DataRow dataRow = dataTable
                .AsEnumerable()
                .FirstOrDefault(dr => dr[columnNameOfPrimaryKey] != DBNull.Value && Convert.ToInt32(dr[columnNameOfPrimaryKey].ToString()) == Convert.ToInt32(source));

            if (dataRow == null)
            {
                return default(TOutput);
            }

            return GlobalHelper.ParseEnum<TOutput>(dataRow[columnNameOfFieldName] as string);
        }

        public const string CacheKeyPrefix = "LookupDataTable";
    }
}
