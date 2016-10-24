using System;

namespace Common.Validation
{
    public interface IValidatorProvider
    {
        IValidator GetValidator(Type type);
    }
}
