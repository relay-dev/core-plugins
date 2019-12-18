using Core.Data;
using System;
using System.Collections.Generic;
using System.Data;

namespace Core.Plugins.Data.Command
{
    public class DatabaseCommandBuilder
    {
        private readonly IDatabase _database;
        private readonly List<DatabaseCommandParameter> _parameters;

        public DatabaseCommandBuilder(IDatabase database)
        {
            _database = database;
            _parameters = new List<DatabaseCommandParameter>();
        }

        public DatabaseCommandBuilder AddInputParameter<TParameter>(string parameterName, TParameter parameterValue)
        {
            var databaseParameter = new DatabaseCommandParameter
            {
                Name = parameterName,
                Value = parameterValue,
                Direction = ParameterDirection.Input
            };

            if (databaseParameter.Value == null)
            {
                databaseParameter.Value = DBNull.Value;
            }

            _parameters.Add(databaseParameter);

            return this;
        }

        public DatabaseCommandBuilder AddInputOutputParameter<TParameter>(string parameterName, TParameter parameterValue)
        {
            var databaseParameter = new DatabaseCommandParameter
            {
                Name = parameterName,
                Value = parameterValue,
                Direction = ParameterDirection.InputOutput
            };

            if (databaseParameter.Value == null)
            {
                databaseParameter.Value = DBNull.Value;
            }

            _parameters.Add(databaseParameter);

            return this;
        }

        public DatabaseCommandBuilder AddOutputParameter(string parameterName, string typeName)
        {
            var databaseParameter = new DatabaseCommandParameter
            {
                Name = parameterName,
                Direction = ParameterDirection.Output,
                TypeName = typeName
            };

            _parameters.Add(databaseParameter);

            return this;
        }

        public DatabaseCommandBuilder AddReturnParameter(string parameterName)
        {
            var databaseParameter = new DatabaseCommandParameter
            {
                Name = parameterName,
                Direction = ParameterDirection.ReturnValue
            };

            _parameters.Add(databaseParameter);

            return this;
        }

        public StoredProcedureCommand ForStoredProcedure(string storedProcedureName)
        {
            return new StoredProcedureCommand(_database, storedProcedureName, _parameters);
        }

        public SQLQueryCommand ForSQLQuery(string sql)
        {
            return new SQLQueryCommand(_database, sql, _parameters);
        }

        public SQLNonQueryCommand ForSQLNonQuery(string sql)
        {
            return new SQLNonQueryCommand(_database, sql, _parameters);
        }
    }
}
