using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace General.Validation
{
    public abstract class ValidatorBase<T> : IValidator<T>
        where T : class
    {
        public Type EntityType { get { return typeof(T); } }

        #region IValidator<T> Members

        public abstract IEnumerable<KeyValuePair<string, string>> Validate(T entity, string propertyName);
        public abstract Task<IEnumerable<KeyValuePair<string, string>>> ValidateAsync(T entity, string propertyName);

        #endregion

        #region IValidator Members

        public IEnumerable<KeyValuePair<string, string>> Validate(object entity, string propertyName)
        {
            if (entity is T)
            {
                return Validate(entity as T, propertyName);
            }

            return new List<KeyValuePair<string, string>>
            {
                new KeyValuePair<string, string>(propertyName, ExceptionMessages.NotRightTypeValidationError)
            };
        }

        public async Task<IEnumerable<KeyValuePair<string, string>>> ValidateAsync(object entity, string propertyName)
        {
            if (entity is T)
            {
                return await ValidateAsync(entity as T, propertyName);
            }

            return new List<KeyValuePair<string, string>>
            {
                new KeyValuePair<string, string>(propertyName, ExceptionMessages.NotRightTypeValidationError)
            };
        }

        #endregion

        protected string GenerateKey(string baseName, string propertyName)
        {
            return $"{baseName}.{propertyName}";
        }
    }
}
