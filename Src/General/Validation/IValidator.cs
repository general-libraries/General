using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace General.Validation
{
    public interface IValidator
    {
        Type EntityType { get; }
        IEnumerable<KeyValuePair<string, string>> Validate(object entity, string paramName);
        Task<IEnumerable<KeyValuePair<string, string>>> ValidateAsync(object entity, string paramName);
    }

    public interface IValidator<T> : IValidator
        where T : class
    {
        IEnumerable<KeyValuePair<string, string>> Validate(T entity, string paramName);
        Task<IEnumerable<KeyValuePair<string, string>>> ValidateAsync(T entity, string paramName);
    }
}
