using Core.Data;
using System;
using System.Collections.Generic;
using System.Data;

namespace Core.Plugins.Data.Command
{
    public abstract class DatabaseCommandWithParameters : DatabaseCommand
    {
        protected readonly List<DatabaseCommandParameter> Parameters;

        public DatabaseCommandWithParameters(IDatabase database) : base(database)
        {
            Parameters = new List<DatabaseCommandParameter>();
        }

        public DatabaseCommandWithParameters(IDatabase database, string target) : base(database, target)
        {
            Parameters = new List<DatabaseCommandParameter>();
        }

        public DatabaseCommandWithParameters AddInputParameter<TParameter>(string parameterName, TParameter parameterValue)
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

        public DatabaseCommandWithParameters AddInputOutputParameter<TParameter>(string parameterName, TParameter parameterValue)
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

        public DatabaseCommandWithParameters AddOutputParameter(string parameterName, string typeName)
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

        public DatabaseCommandWithParameters AddReturnParameter(string parameterName)
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
    }
}
