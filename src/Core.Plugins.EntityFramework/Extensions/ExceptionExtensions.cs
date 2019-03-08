using System.Data.Entity.Validation;
using System.Text;

namespace Core.Plugins.EntityFramework.Extensions
{
    public static class ExceptionExtensions
    {
        public static string GetEntityValidationErrors(this DbEntityValidationException e)
        {
            var stringBuilder = new StringBuilder();

            stringBuilder.AppendLine("Entity Framework encountered the following errors:");

            foreach (var validationErrors in e.EntityValidationErrors)
            {
                foreach (var validationError in validationErrors.ValidationErrors)
                {
                    stringBuilder.AppendLine(validationError.ErrorMessage.Contains(validationError.PropertyName)
                        ? validationError.ErrorMessage
                        : $"{validationError.ErrorMessage} (PropertyName: '{validationError.PropertyName}')");
                }
            }

            return stringBuilder.ToString();
        }

        public static string GetEntityValidationErrorsToDisplay(this DbEntityValidationException e)
        {
            var stringBuilder = new StringBuilder();

            foreach (var validationErrors in e.EntityValidationErrors)
            {
                foreach (var validationError in validationErrors.ValidationErrors)
                {
                    stringBuilder.AppendLine($"Property: {validationError.PropertyName} Error: {validationError.ErrorMessage}");
                }
            }

            return stringBuilder.ToString();
        }
    }
}
