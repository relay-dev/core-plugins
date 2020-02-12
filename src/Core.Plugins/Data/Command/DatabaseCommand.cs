using Core.Data;
using Core.Framework;
using System;
using System.Collections.Generic;
using System.Data;

namespace Core.Plugins.Data.Command
{
    public abstract class DatabaseCommand : ICommand<DatabaseCommandResult>
    {
        protected readonly IDatabase Database;
        protected readonly string Target;
        protected readonly List<DatabaseCommandParameter> Parameters;

        public DatabaseCommand(IDatabase database)
        {
            Database = database;
            Parameters = new List<DatabaseCommandParameter>();
        }

        public DatabaseCommand(IDatabase database, string target)
        {
            Database = database;
            Target = target;
            Parameters = new List<DatabaseCommandParameter>();
        }

        public DatabaseCommand AddInputParameter<TParameter>(string parameterName, TParameter parameterValue)
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

            Parameters.Add(databaseParameter);

            return this;
        }

        public DatabaseCommand AddInputOutputParameter<TParameter>(string parameterName, TParameter parameterValue)
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

            Parameters.Add(databaseParameter);

            return this;
        }

        public DatabaseCommand AddOutputParameter(string parameterName, string typeName)
        {
            var databaseParameter = new DatabaseCommandParameter
            {
                Name = parameterName,
                Direction = ParameterDirection.Output,
                TypeName = typeName
            };

            Parameters.Add(databaseParameter);

            return this;
        }

        public DatabaseCommand AddReturnParameter(string parameterName)
        {
            var databaseParameter = new DatabaseCommandParameter
            {
                Name = parameterName,
                Direction = ParameterDirection.ReturnValue
            };

            Parameters.Add(databaseParameter);

            return this;
        }

        public List<DatabaseCommandParameter> GetParameters()
        {
            return Parameters;
        }

        public abstract DatabaseCommandResult Execute();
    }
}
