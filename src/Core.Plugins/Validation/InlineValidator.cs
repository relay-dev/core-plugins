using Core.Exceptions;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace Core.Validation
{
    public class InlineValidator : IValidatorInline
    {
        private readonly List<string> _errors;

        public InlineValidator()
        {
            _errors = new List<string>();
        }

        public IValidatorInline Not<T>(ExpressionType expressionType, Expression<Func<T>> argExpression, T value) where T : IComparable
        {
            return Not(Expression.Lambda<Func<bool>>(Expression.MakeBinary(expressionType, argExpression.Body, Expression.Constant(value))));
        }

        public IValidatorInline Not(Expression<Func<bool>> expression)
        {
            if (expression.Compile()())
            {
                var bodyExpression = expression.Body;
                string argumentMessage = String.Empty;
                string name = String.Empty;

                if (bodyExpression is BinaryExpression binaryExpression)
                {
                    var leftExpression = binaryExpression.Left;
                    var rightExpression = binaryExpression.Right;

                    object value;
                    if ((name = GetNameFromExpression(leftExpression)) != null)
                    {
                        value = GetValue(rightExpression);
                    }
                    else
                    {
                        name = GetNameFromExpression(rightExpression);
                        value = GetValue(rightExpression);
                    }

                    argumentMessage = String.Format("{0} is {1} {2} which is invalid.", name, GetNode(binaryExpression), value == null ? "null" : value.ToString());
                }
                
                if (bodyExpression is MethodCallExpression methodExpression)
                {
                    var methodName = methodExpression.Method.Name;
                    var parameterNames = methodExpression.Arguments.Select(e => GetNameFromExpression(e)).ToList();
                    var values = methodExpression.Arguments.Select(e =>
                    {
                        var val = GetValue(e);
                        return val == null ? "null" : val.ToString();
                    }).ToList();

                    argumentMessage = String.Format(CultureInfo.InvariantCulture, "{0} was {1} while calling {2} which is invalid.", string.Join(", ", parameterNames), string.Join(", ", values), methodName);
                }

                _errors.Add(argumentMessage);
            }

            return this;
        }

        public IValidatorInline NotNull(Expression<Func<object>> argExpression)
        {
            var value = argExpression.Compile()();

            if (value == null)
            {
                string argumentName = GetNameFromExpression(argExpression.Body);
                _errors.Add($"{argumentName} cannot be null");
            }

            return this;
        }

        public IValidatorInline NotNullOrEmpty(Expression<Func<string>> argExpression)
        {
            var value = argExpression.Compile()();

            if (String.IsNullOrEmpty(value))
            {
                string argumentName = GetNameFromExpression(argExpression.Body);

                _errors.Add($"{argumentName} cannot be null or empty");
            }

            return this;
        }

        public IValidatorInline NotNullOrEmptyCollection<T>(Expression<Func<ICollection<T>>> argExpression)
        {
            var value = argExpression.Compile()();
            string argumentName = GetNameFromExpression(argExpression.Body);

            if (value == null)
            {
                _errors.Add($"{argumentName} cannot be null");
            }
            else if (!value.Any())
            {
                _errors.Add($"{argumentName} cannot be empty");
            }

            return this;
        }

        public IValidatorInline NotDefault<T>(Expression<Func<T>> argExpression)
        {
            var value = argExpression.Compile()();
            string argumentName = GetNameFromExpression(argExpression.Body);

            T comparason = default(T);

            if (ReferenceEquals(value, null))
            {
                _errors.Add($"{argumentName} cannot be null");
            }
            
            if (value.Equals(comparason))
            {
                string compare = (comparason == null ? "null" : comparason.ToString());

                _errors.Add($"{argumentName} cannot be {compare}");
            }

            return this;
        }

        public IValidatorInline NotInvalidID(Expression<Func<long>> argExpression)
        {
            if (argExpression.Compile().Invoke() < 1)
            {
                string argumentName = GetNameFromUnaryExpression(argExpression.Body);

                _errors.Add($"{argumentName} must be greater than 0");
            }

            return this;
        }

        public IValidatorInline NotEnumUndefined(Expression<Func<Enum>> argExpression)
        {
            var value = argExpression.Compile()();

            if (value != null && value.ToString().ToLower() == "undefined")
            {
                string argumentName = GetNameFromUnaryExpression(argExpression.Body);

                _errors.Add($"{argumentName} cannot be set to Undefined");
            }

            return this;
        }

        public IValidatorInline NotGreaterThan<T>(Expression<Func<T>> argExpression, T value) where T : IComparable
        {
            return Not(ExpressionType.GreaterThan, argExpression, value);
        }

        public IValidatorInline NotGreaterThanOrEqual<T>(Expression<Func<T>> argExpression, T value) where T : IComparable
        {
            return Not(ExpressionType.GreaterThanOrEqual, argExpression, value);
        }

        public IValidatorInline NotLessThan<T>(Expression<Func<T>> argExpression, T value) where T : IComparable
        {
            return Not(ExpressionType.LessThan, argExpression, value);
        }

        public IValidatorInline NotLessThanOrEqual<T>(Expression<Func<T>> argExpression, T value) where T : IComparable
        {
            return Not(ExpressionType.LessThanOrEqual, argExpression, value);
        }

        public void Validate()
        {
            if (_errors.Any())
            {
                string errorMessage = String.Join(Environment.NewLine, _errors);

                throw new CoreException(ErrorCode.INVA, $"Encountered the following errors: {errorMessage}");
            }
        }

        private object GetValue(Expression expression)
        {
            if (expression is ConstantExpression constantExpression)
                return constantExpression.Value;

            if (expression is UnaryExpression unaryExpression)
                return GetValue(unaryExpression.Operand);

            if (expression is MethodCallExpression invocationExpression)
                return invocationExpression.Method.Invoke(GetValue(invocationExpression.Object), invocationExpression.Arguments.Select(GetValue).ToArray());

            if (expression is MemberExpression memberExpression)
            {
                var propertyInfo = memberExpression.Member as PropertyInfo;
                if (propertyInfo != null)
                    return propertyInfo.GetValue(GetValue(memberExpression.Expression), null);

                var fieldInfo = memberExpression.Member as FieldInfo;
                if (fieldInfo != null)
                    return fieldInfo.GetValue(GetValue(memberExpression.Expression));

                var methodInfo = memberExpression.Member as MethodInfo;
                if (methodInfo != null)
                    return methodInfo.Invoke(GetValue(memberExpression.Expression), new object[0]);
            }

            return null;
        }

        private string GetNode(BinaryExpression binaryExpression)
        {
            switch (binaryExpression.NodeType)
            {
                case ExpressionType.And:
                case ExpressionType.AndAlso:
                case ExpressionType.Constant:
                case ExpressionType.GreaterThan:
                case ExpressionType.GreaterThanOrEqual:
                case ExpressionType.IsFalse:
                case ExpressionType.IsTrue:
                case ExpressionType.LessThan:
                case ExpressionType.LessThanOrEqual:
                case ExpressionType.Not:
                case ExpressionType.Or:
                case ExpressionType.OrElse:
                    return binaryExpression.NodeType.ToString();
                case ExpressionType.Equal:
                case ExpressionType.NotEqual:
                    return binaryExpression.NodeType + " to";
            }

            return binaryExpression.NodeType.ToString();
        }

        private string GetNameFromExpression(Expression argExpression, string name = null)
        {
            var memberExpression = argExpression as MemberExpression;

            if (memberExpression != null)
            {
                if (name == null)
                    name = string.Empty;
                name += GetNameFromExpression(memberExpression.Expression);
                if (name != string.Empty)
                    name += ".";
                name += memberExpression.Member.Name;
            }

            return ToUpperFirstLetter(name);
        }

        private string GetNameFromUnaryExpression(Expression argExpression, string name = null)
        {
            if (!(argExpression is UnaryExpression unaryExpression))
                return String.Empty;

            if (!(unaryExpression.Operand is MemberExpression memberExpression))
                return unaryExpression.Operand.Type.Name;

            return ToUpperFirstLetter(memberExpression.Member.Name);
        }

        private string ToUpperFirstLetter(string str)
        {
            if (str == null)
                return null;

            if (str.Length > 1)
                return char.ToUpper(str[0]) + str.Substring(1).ToLower();

            return str.ToUpper();
        }
    }
}
